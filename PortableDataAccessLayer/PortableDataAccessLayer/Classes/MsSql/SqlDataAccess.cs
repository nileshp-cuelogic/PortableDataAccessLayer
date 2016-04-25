using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;

namespace PortableDataAccessLayer
{

    internal class SqlDataAccess : IDataAccess
    {
        private string _ConnectionString = "";

        const int CommandTimeout = 300;

        public SqlDataAccess() { }

        public SqlDataAccess(string ConnectionString)
        {
            this._ConnectionString = ConnectionString;
        }

        private SqlConnection _sqlConnection { get; set; }

        private SqlDataAdapter GetSqlDataAdapter(string commandText = "")
        {
            if (!IsConnectionStringValid())
                throw new Exception("Connection string not set correctly!");

            if (this._sqlConnection == null)
                this._sqlConnection = new SqlConnection(this._ConnectionString);
            else
            {
                this._sqlConnection.Dispose();
                this._sqlConnection = null;
                this._sqlConnection = new SqlConnection(this._ConnectionString);
            }

            return new SqlDataAdapter(commandText, this._sqlConnection);
        }


        private SqlCommand GetSqlCommand(string commandText = "")
        {
            if (!IsConnectionStringValid())
                throw new Exception("Connection string not set correctly!");

            if (this._sqlConnection == null)
                this._sqlConnection = new SqlConnection(this._ConnectionString);
            else
            {
                this._sqlConnection.Dispose();
                this._sqlConnection = null;
                this._sqlConnection = new SqlConnection(this._ConnectionString);
            }

            return new SqlCommand(commandText, this._sqlConnection);
        }

        private void IsValidCommandText(string commandText)
        {
            if (commandText == null || commandText.Trim() == "")
            {
                throw new Exception("Invalid Command Text");
            }
        }

        private bool IsConnectionStringValid()
        {
            return !String.IsNullOrEmpty(this._ConnectionString);
        }



        #region ExecuteScaler
        public object ExecuteScaler(string commandText)
        {
            return ExecuteScaler(commandText, CommandType.Text, (DalParameterList)null);
        }

        public object ExecuteScaler(string commandText, CommandType commandType)
        {
            return ExecuteScaler(commandText, commandType, (DalParameterList)null);
        }

        public object ExecuteScaler(string commandText, CommandType commandType, DalParameterList dalParameterList)
        {
            IsValidCommandText(commandText);
            DataSet TempDataSet = new DataSet();
            SqlDataAdapter _sqlDataAdapter = null;
            int OutputParametersCount = 0; // count Output parameters sent to stored procedure

            try
            {

                // get new sql data adapter
                _sqlDataAdapter = GetSqlDataAdapter(commandText);

                using (_sqlDataAdapter)
                {
                    // prepare procedure
                    _sqlDataAdapter.SelectCommand.CommandType = commandType;
                    _sqlDataAdapter.SelectCommand.CommandTimeout = CommandTimeout;

                    // add sql parameters to procedure
                    if (dalParameterList != null)
                    {
                        foreach (DalParameter DalParam in dalParameterList)
                        {
                            if (DalParam.ParameterDirection == ParameterDirection.Output)
                                OutputParametersCount++;

                            SqlParameter SqlParam = new SqlParameter()
                            {
                                ParameterName = DalParam.ParameterName,
                                SqlDbType = DalParam.ParameterType,
                                Size = DalParam.ParameterSize,
                                Direction = DalParam.ParameterDirection,
                                SqlValue = DalParam.ParameterValue
                            };

                            _sqlDataAdapter.SelectCommand.Parameters.Add(SqlParam);
                        }
                    }

                    // retrieve data into datasets from stored procedure
                    _sqlDataAdapter.Fill(TempDataSet);


                    // handle output parameters
                    if (OutputParametersCount > 0)
                    {
                        foreach (SqlParameter SqlParam in _sqlDataAdapter.SelectCommand.Parameters)
                        {
                            if (SqlParam.Direction == ParameterDirection.Output)
                            {
                                dalParameterList.Find((x) => x.ParameterName == SqlParam.ParameterName
                                                    && x.ParameterDirection == ParameterDirection.Output)
                                                    .ParameterValue = SqlParam.Value;
                            }
                        }
                    }//if(iOutputParametersCount > 0)

                }



                object ReturnValue = null;

                if (TempDataSet != null && TempDataSet.Tables.Count > 0 && TempDataSet.Tables[0].Rows.Count > 0)
                {
                    ReturnValue = TempDataSet.Tables[0].Rows[0][0];
                }

                return ReturnValue;
            }
            catch (SqlException SqlEx)
            {
                throw SqlEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_sqlDataAdapter != null)
                    _sqlDataAdapter.Dispose();

                if (_sqlConnection != null)
                    _sqlConnection.Dispose();

            }
        }
        #endregion

        #region ExecuteNonQuery
        public int ExecuteNonQuery(string commandText)
        {
            return ExecuteNonQuery(commandText, CommandType.Text, (DalParameterList)null);
        }

        public int ExecuteNonQuery(string commandText, CommandType commandType)
        {
            return ExecuteNonQuery(commandText, commandType, (DalParameterList)null);
        }

        public int ExecuteNonQuery(string commandText, CommandType commandType, DalParameterList dalParameterList)
        {
            IsValidCommandText(commandText);
            int RecordsAffected = -1;
            SqlCommand _sqlCommand = null;

            int OutputParametersCount = 0; // count Output parameters sent to stored procedure
            bool HasTransactionBegan = false;

            try
            {
                // get new sql command
                _sqlCommand = GetSqlCommand(commandText);

                using (_sqlCommand)
                {
                    // prepare procedure
                    _sqlCommand.CommandType = commandType;
                    _sqlCommand.CommandTimeout = CommandTimeout;

                    // add sql parameters to procedure
                    if (dalParameterList != null)
                    {
                        foreach (DalParameter DalParam in dalParameterList)
                        {
                            if (DalParam.ParameterDirection == ParameterDirection.Output)
                                OutputParametersCount++;

                            SqlParameter SqlParam = new SqlParameter()
                            {
                                ParameterName = DalParam.ParameterName,
                                SqlDbType = DalParam.ParameterType,
                                Direction = DalParam.ParameterDirection,
                                SqlValue = DalParam.ParameterValue
                            };

                            _sqlCommand.Parameters.Add(SqlParam);
                        }
                    }

                    // take care of transaction business

                    _sqlCommand.Connection.Open();

                    _sqlCommand.Transaction = _sqlCommand.Connection.BeginTransaction();
                    HasTransactionBegan = true;

                    // executes procedure to insert/update/delete data
                    RecordsAffected = _sqlCommand.ExecuteNonQuery();

                    _sqlCommand.Transaction.Commit();

                    // handle output parameters
                    if (OutputParametersCount > 0)
                    {
                        foreach (SqlParameter SqlParam in _sqlCommand.Parameters)
                        {
                            if (SqlParam.Direction == ParameterDirection.Output)
                            {
                                dalParameterList.Find((x) => x.ParameterName == SqlParam.ParameterName
                                                    && x.ParameterDirection == ParameterDirection.Output)
                                                    .ParameterValue = SqlParam.Value;
                            }
                        }
                    }
                }//using SureScoreCommand
            }
            catch (SqlException SqlEx) { throw SqlEx; }
            catch (Exception ex) { throw ex; }
            finally
            {
                if (_sqlCommand != null && _sqlCommand.Transaction != null && HasTransactionBegan)
                    _sqlCommand.Transaction.Rollback();

                if (_sqlCommand.Connection.State != ConnectionState.Closed)
                    _sqlCommand.Connection.Close();

                //if (SureScoreCommand != null)
                // SureScoreCommand.Dispose();

                //if (SureScoreConnection != null)
                //  SureScoreConnection.Dispose();
            }

            return RecordsAffected;
        }
        #endregion

        #region ExecuteDataReader
        public DbDataReader ExecuteDataReader(string commandText)
        {
            return ExecuteDataReader(commandText, CommandType.Text, (DalParameterList)null);
        }

        public DbDataReader ExecuteDataReader(string commandText, CommandType commandType)
        {
            return ExecuteDataReader(commandText, commandType, (DalParameterList)null);
        }


        public DbDataReader ExecuteDataReader(string commandText, CommandType commandType, DalParameterList dalParameterList)
        {
            IsValidCommandText(commandText);
            DbDataReader dbDataReader = null;

            SqlCommand _sqlCommand = null;

            int OutputParametersCount = 0; // count Output parameters sent to stored procedure

            try
            {
                // get new sql command
                _sqlCommand = GetSqlCommand(commandText);

                using (_sqlCommand)
                {
                    // prepare procedure
                    _sqlCommand.CommandType = commandType;
                    _sqlCommand.CommandTimeout = CommandTimeout;

                    // add sql parameters to procedure
                    if (dalParameterList != null)
                    {
                        foreach (DalParameter DalParam in dalParameterList)
                        {
                            if (DalParam.ParameterDirection == ParameterDirection.Output)
                                OutputParametersCount++;

                            SqlParameter SqlParam = new SqlParameter()
                            {
                                ParameterName = DalParam.ParameterName,
                                SqlDbType = DalParam.ParameterType,
                                Direction = DalParam.ParameterDirection,
                                SqlValue = DalParam.ParameterValue
                            };

                            _sqlCommand.Parameters.Add(SqlParam);
                        }
                    }

                    // take care of transaction business

                    _sqlCommand.Connection.Open();


                    // executes procedure to insert/update/delete data
                    dbDataReader = _sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);

                    // handle output parameters
                    if (OutputParametersCount > 0)
                    {
                        foreach (SqlParameter SqlParam in _sqlCommand.Parameters)
                        {
                            if (SqlParam.Direction == ParameterDirection.Output)
                            {
                                dalParameterList.Find((x) => x.ParameterName == SqlParam.ParameterName
                                                    && x.ParameterDirection == ParameterDirection.Output)
                                                    .ParameterValue = SqlParam.Value;
                            }
                        }
                    }
                }//using SureScoreCommand
            }
            catch (SqlException SqlEx) { throw SqlEx; }
            catch (Exception ex) { throw ex; }
            finally
            {

                //if (_sqlCommand.Connection.State != ConnectionState.Closed)
                //    _sqlCommand.Connection.Close();

                //if (SureScoreCommand != null)
                // SureScoreCommand.Dispose();

                //if (SureScoreConnection != null)
                //  SureScoreConnection.Dispose();
            }

            return dbDataReader;
        }
        #endregion

        #region ExecuteDataTable
        public DataTable ExecuteDataTable(string commandText, string TableName = null)
        {
            return ExecuteDataTable(commandText, CommandType.Text, (DalParameterList)null, TableName);
        }

        public DataTable ExecuteDataTable(string commandText, CommandType commandType, string TableName = null)
        {
            return ExecuteDataTable(commandText, commandType, (DalParameterList)null, TableName);
        }

        public DataTable ExecuteDataTable(string commandText, CommandType commandType, DalParameterList dalParameterList, string TableName = null)
        {
            IsValidCommandText(commandText);
            DataSet TempDataSet = new DataSet();
            SqlDataAdapter _sqlDataAdapter = null;
            int OutputParametersCount = 0; // count Output parameters sent to stored procedure
            try
            {

                // get new sql data adapter
                _sqlDataAdapter = GetSqlDataAdapter(commandText);

                using (_sqlDataAdapter)
                {
                    // prepare procedure
                    _sqlDataAdapter.SelectCommand.CommandType = commandType;
                    _sqlDataAdapter.SelectCommand.CommandTimeout = CommandTimeout;


                    // add sql parameters to procedure
                    if (dalParameterList != null)
                    {
                        foreach (DalParameter DalParam in dalParameterList)
                        {
                            if (DalParam.ParameterDirection == ParameterDirection.Output)
                                OutputParametersCount++;

                            SqlParameter SqlParam = new SqlParameter()
                            {
                                ParameterName = DalParam.ParameterName,
                                SqlDbType = DalParam.ParameterType,
                                Size = DalParam.ParameterSize,
                                Direction = DalParam.ParameterDirection,
                                SqlValue = DalParam.ParameterValue
                            };

                            _sqlDataAdapter.SelectCommand.Parameters.Add(SqlParam);
                        }
                    }

                    // retrieve data into datasets from stored procedure
                    _sqlDataAdapter.Fill(TempDataSet);


                    // handle output parameters
                    if (OutputParametersCount > 0)
                    {
                        foreach (SqlParameter SqlParam in _sqlDataAdapter.SelectCommand.Parameters)
                        {
                            if (SqlParam.Direction == ParameterDirection.Output)
                            {
                                dalParameterList.Find((x) => x.ParameterName == SqlParam.ParameterName
                                                    && x.ParameterDirection == ParameterDirection.Output)
                                                    .ParameterValue = SqlParam.Value;
                            }
                        }
                    }//if(iOutputParametersCount > 0)

                }

                DataTable ReturnValue = null;

                if (TempDataSet != null && TempDataSet.Tables.Count > 0)
                {
                    ReturnValue = TempDataSet.Tables[0];
                    if (TableName != null && TableName.Trim() != "")
                    {
                        ReturnValue.TableName = TableName;
                    }
                }

                return ReturnValue;
            }
            catch (SqlException SqlEx)
            {
                throw SqlEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_sqlDataAdapter != null)
                    _sqlDataAdapter.Dispose();

                if (_sqlConnection != null)
                    _sqlConnection.Dispose();

            }
        }
        #endregion

        #region ExecuteDataSet
        public DataSet ExecuteDataSet(string commandText, string[] TableNames = null)
        {
            return ExecuteDataSet(commandText, CommandType.Text, (DalParameterList)null, TableNames);
        }

        public DataSet ExecuteDataSet(string commandText, CommandType commandType, string[] TableNames = null)
        {
            return ExecuteDataSet(commandText, commandType, (DalParameterList)null, TableNames);
        }

        public DataSet ExecuteDataSet(string commandText, CommandType commandType, DalParameterList dalParameterList, string[] TableNames = null)
        {
            IsValidCommandText(commandText);
            DataSet TempDataSet = new DataSet();
            SqlDataAdapter _sqlDataAdapter = null;
            int OutputParametersCount = 0; // count Output parameters sent to stored procedure
            try
            {

                // get new sql data adapter
                _sqlDataAdapter = GetSqlDataAdapter(commandText);

                using (_sqlDataAdapter)
                {
                    // prepare procedure
                    _sqlDataAdapter.SelectCommand.CommandType = commandType;
                    _sqlDataAdapter.SelectCommand.CommandTimeout = CommandTimeout;

                    // add sql parameters to procedure
                    if (dalParameterList != null)
                    {
                        foreach (DalParameter DalParam in dalParameterList)
                        {
                            if (DalParam.ParameterDirection == ParameterDirection.Output)
                                OutputParametersCount++;

                            SqlParameter SqlParam = new SqlParameter()
                            {
                                ParameterName = DalParam.ParameterName,
                                SqlDbType = DalParam.ParameterType,
                                Size = DalParam.ParameterSize,
                                Direction = DalParam.ParameterDirection,
                                SqlValue = DalParam.ParameterValue
                            };

                            _sqlDataAdapter.SelectCommand.Parameters.Add(SqlParam);
                        }
                    }

                    // retrieve data into datasets from stored procedure
                    _sqlDataAdapter.Fill(TempDataSet);


                    // handle output parameters
                    if (OutputParametersCount > 0)
                    {
                        foreach (SqlParameter SqlParam in _sqlDataAdapter.SelectCommand.Parameters)
                        {
                            if (SqlParam.Direction == ParameterDirection.Output)
                            {
                                dalParameterList.Find((x) => x.ParameterName == SqlParam.ParameterName
                                                    && x.ParameterDirection == ParameterDirection.Output)
                                                    .ParameterValue = SqlParam.Value;
                            }
                        }
                    }//if(iOutputParametersCount > 0)

                }



                if (TempDataSet != null && TempDataSet.Tables.Count > 0)
                {
                    int counter = 0;
                    int tableNamesCount = TableNames != null ? TableNames.Length : 0;
                    foreach (DataTable table in TempDataSet.Tables)
                    {
                        if (counter < tableNamesCount)
                        {
                            table.TableName = TableNames[counter].Trim() == "" ? table.TableName : TableNames[counter];
                            counter++;
                        }
                    }

                }

                return TempDataSet;
            }
            catch (SqlException SqlEx)
            {
                throw SqlEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_sqlDataAdapter != null)
                    _sqlDataAdapter.Dispose();

                if (_sqlConnection != null)
                    _sqlConnection.Dispose();

            }
        }
        #endregion
    }
}
