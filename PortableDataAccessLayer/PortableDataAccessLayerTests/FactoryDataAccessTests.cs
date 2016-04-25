using Microsoft.VisualStudio.TestTools.UnitTesting;
using PortableDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableDataAccessLayer.Tests
{
    /// <exclude />
    [TestClass()]
    public class FactoryDataAccessTests
    {
        string _validConnectionString = "Data Source=TEST-PC;Initial Catalog=kantarPractice;Integrated Security=False;User ID=sa;Password=pa$$word;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        string _inValidConnectionString = "sssssss";
        string _emptyConnectionString = "    ";
        string _nullConnectionString = null;
        string _emptyCommandText = "";
        string _nullCommandText = null;


        string _validCommandText = "select * From city";
        string _validDmlCommandText = "INSERT INTO Country VALUES('nilesh')";
        string _invalidDmlCommandText = "INSERT INTO";
        string _validCommandTextWithParameter = "select * From city where id=@p1";
        string _validDmlCommandTextWithParameter = "Insert into Country VALUES(@p1)";



        #region  Get Data Access
        /// <exclude />
        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void GetDataAccessTestWithEmptyConnectionString()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _emptyConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();

        }

        /// <exclude />
        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void GetDataAccessTestWithNullConnectionString()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _nullConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();

        }


        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void GetDataAccessTestWithInvalidConnectionString()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _inValidConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();

        }

        /// <exclude />
        [TestMethod()]
        public void GetDataAccessTestWithValidConnectionString()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            Assert.IsTrue(objDataAccess != null);

        }
        #endregion


        #region Execute Scalor
        /// <exclude />
        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void ExecuteScalerTestWithEmptyCommandText()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            object retVal = objDataAccess.ExecuteScaler(_emptyCommandText);
        }

        /// <exclude />
        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void ExecuteScalerTestWithNullCommandText()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            object retVal = objDataAccess.ExecuteScaler(_nullCommandText);
        }

        /// <exclude />
        [TestMethod()]
        public void ExecuteScalerTestWithValidCommandText()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            object retVal = objDataAccess.ExecuteScaler(_validCommandText);
            Assert.IsTrue(retVal != null);
        }

        /// <exclude />
        [TestMethod()]
        public void ExecuteScalerTestWithDMLCommandText()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            object retVal = objDataAccess.ExecuteScaler(_validDmlCommandText);
            Assert.IsTrue(retVal == null);
        }


        /// <exclude />
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExecuteScalerTestWithInValidCommandType()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            //CommandType.TableDirect is not valid command type for select command
            object retVal = objDataAccess.ExecuteScaler(_validCommandText, CommandType.TableDirect);
            Assert.IsTrue(retVal != null);
        }

        /// <exclude />
        [TestMethod()]
        public void ExecuteScalerTestWithValidCommandType()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();

            object retVal = objDataAccess.ExecuteScaler(_validCommandText, CommandType.Text);
            Assert.IsTrue(retVal != null);
        }

        /// <exclude />
        [TestMethod()]
        public void ExecuteScalerTestWithNullParameterList()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = null;

            object retVal = objDataAccess.ExecuteScaler(_validCommandText, CommandType.Text, dal);
            Assert.IsTrue(retVal != null);
        }

        /// <exclude />
        [TestMethod()]
        public void ExecuteScalerTestWithEmptyParameterList()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = new DalParameterList();

            object retVal = objDataAccess.ExecuteScaler(_validCommandText, CommandType.Text, dal);
            Assert.IsTrue(retVal != null);
        }

        /// <exclude />
        [TestMethod()]
        [ExpectedException(typeof(SqlException))]
        public void ExecuteScalerTestWithInvalidParameterList()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = new DalParameterList();
            dal.Add(new DalParameter()
            {
                ParameterName = "",
                ParameterDirection = ParameterDirection.Input
            });




            object retVal = objDataAccess.ExecuteScaler(_validCommandTextWithParameter, CommandType.Text, dal);
            Assert.IsTrue(retVal != null);
        }

        /// <exclude />
        [TestMethod()]
        [ExpectedException(typeof(SqlException))]
        public void ExecuteScalerTestWithoutInputParameterValue()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = new DalParameterList();


            //without Parameter value
            dal.Add(new DalParameter()
            {
                ParameterName = "@p1",
                ParameterDirection = ParameterDirection.Input
            });


            object retVal = objDataAccess.ExecuteScaler(_validCommandTextWithParameter, CommandType.Text, dal);
            Assert.IsTrue(retVal != null);
        }

        /// <exclude />
        [TestMethod()]
        [ExpectedException(typeof(SqlException))]
        public void ExecuteScalerTestWithInvalidParameterDirection()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = new DalParameterList();


            //without Parameter value
            dal.Add(new DalParameter()
            {
                ParameterName = "@p1",
                ParameterValue = 2,
                ParameterDirection = ParameterDirection.ReturnValue
            });


            object retVal = objDataAccess.ExecuteScaler(_validCommandTextWithParameter, CommandType.Text, dal);
            Assert.IsTrue(retVal == null);
        }

        /// <exclude />
        [TestMethod()]
        public void ExecuteScalerTestWithInvalidParameterDirectionOutput()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = new DalParameterList();


            //without Parameter value
            dal.Add(new DalParameter()
            {
                ParameterName = "@p1",
                ParameterValue = 2,
                ParameterDirection = ParameterDirection.Output
            });


            object retVal = objDataAccess.ExecuteScaler(_validCommandTextWithParameter, CommandType.Text, dal);
            Assert.IsTrue(retVal == null);
        }

        /// <exclude />
        [TestMethod()]
        public void ExecuteScalerTestWithValidParameter()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = new DalParameterList();


            //without Parameter value
            dal.Add(new DalParameter()
            {
                ParameterName = "@p1",
                ParameterValue = 2,
                ParameterDirection = ParameterDirection.Input

            });

            object retVal = objDataAccess.ExecuteScaler(_validCommandTextWithParameter, CommandType.Text, dal);
            Assert.IsTrue(retVal != null);
        }


        #endregion


        #region ExecuteNonQuery
        /// <exclude />
        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void ExecuteNonQueryTestWithEmptyCommandText()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            int RowsAffected = objDataAccess.ExecuteNonQuery(_emptyCommandText);
        }

        /// <exclude />
        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void ExecuteNonQueryTestWithNullCommandText()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            int RowsAffected = objDataAccess.ExecuteNonQuery(_nullCommandText);
        }

        /// <exclude />
        [TestMethod()]
        public void ExecuteNonQueryTestWithValidCommandText()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            int RowsAffected = objDataAccess.ExecuteNonQuery(_validDmlCommandText);
            Assert.IsTrue(RowsAffected > 0);
        }

        /// <exclude />
        [TestMethod()]
        [ExpectedException(typeof(SqlException))]
        public void ExecuteNonQueryTestWithInValidCommandText()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            int RowsAffected = objDataAccess.ExecuteNonQuery(_invalidDmlCommandText);
            Assert.IsTrue(RowsAffected > 0);
        }

        /// <exclude />
        [TestMethod()]
        public void ExecuteNonQueryTestWithSelectCommandText()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            int RowsAffected = objDataAccess.ExecuteNonQuery(_validCommandText);
            Assert.IsTrue(RowsAffected == -1);
        }


        /// <exclude />
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExecuteNonQueryTestWithInValidCommandType()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            //CommandType.TableDirect is not valid command type for select command
            int RowsAffected = objDataAccess.ExecuteNonQuery(_validDmlCommandText, CommandType.TableDirect);
            Assert.IsTrue(RowsAffected == -1);
        }

        /// <exclude />
        [TestMethod()]
        public void ExecuteNonQueryTestWithValidCommandType()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            int retVal = objDataAccess.ExecuteNonQuery(_validDmlCommandText, CommandType.Text);
            Assert.IsTrue(retVal > 0);
        }

        /// <exclude />
        [TestMethod()]
        public void ExecuteNonQueryTestWithNullParameterList()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = null;

            int retVal = objDataAccess.ExecuteNonQuery(_validDmlCommandText, CommandType.Text, dal);
            Assert.IsTrue(retVal > 0);
        }

        /// <exclude />
        [TestMethod()]
        public void ExecuteNonQueryTestWithEmptyParameterList()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = new DalParameterList();

            int retVal = objDataAccess.ExecuteNonQuery(_validDmlCommandText, CommandType.Text, dal);
            Assert.IsTrue(retVal > 0);
        }

        /// <exclude />
        [TestMethod()]
        [ExpectedException(typeof(SqlException))]
        public void ExecuteNonQueryTestWithInvalidParameterList()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = new DalParameterList();
            dal.Add(new DalParameter()
            {
                ParameterName = "",
                ParameterDirection = ParameterDirection.Input
            });

            int retVal = objDataAccess.ExecuteNonQuery(_validDmlCommandTextWithParameter, CommandType.Text, dal);
            Assert.IsTrue(retVal > 0);
        }

        /// <exclude />
        [TestMethod()]
        [ExpectedException(typeof(SqlException))]
        public void ExecuteNonQueryTestWithoutInputParameterValue()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = new DalParameterList();


            //without Parameter value
            dal.Add(new DalParameter()
            {
                ParameterName = "@p1",
                ParameterDirection = ParameterDirection.Input
            });


            int retVal = objDataAccess.ExecuteNonQuery(_validDmlCommandTextWithParameter, CommandType.Text, dal);
            Assert.IsTrue(retVal > 0);
        }

        /// <exclude />
        [TestMethod()]
        [ExpectedException(typeof(SqlException))]
        public void ExecuteNonQueryTestWithInvalidParameterDirection()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = new DalParameterList();


            //without Parameter value
            dal.Add(new DalParameter()
            {
                ParameterName = "@p1",
                ParameterValue = "Country222",
                ParameterDirection = ParameterDirection.ReturnValue
            });

            int retVal = objDataAccess.ExecuteNonQuery(_validDmlCommandTextWithParameter, CommandType.Text, dal);
            Assert.IsTrue(retVal == -1);
        }

        /// <exclude />
        [TestMethod()]
        [ExpectedException(typeof(FormatException))]
        public void ExecuteNonQueryTestWithWithoutParameterType()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = new DalParameterList();


            //without Parameter value
            dal.Add(new DalParameter()
            {
                ParameterName = "@p1",
                ParameterValue = "Country1",
                ParameterDirection = ParameterDirection.Input
            });


            int retVal = objDataAccess.ExecuteNonQuery(_validDmlCommandTextWithParameter, CommandType.Text, dal);
            Assert.IsTrue(retVal > 0);
        }


        /// <exclude />
        [TestMethod()]
        public void ExecuteNonQueryTestWithWithValidParameterType()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = new DalParameterList();


            //without Parameter value
            dal.Add(new DalParameter()
            {
                ParameterName = "@p1",
                ParameterValue = "Country1",
                ParameterDirection = ParameterDirection.Input,
                ParameterType = SqlDbType.VarChar
            });

            int retVal = objDataAccess.ExecuteNonQuery(_validDmlCommandTextWithParameter, CommandType.Text, dal);
            Assert.IsTrue(retVal > 0);
        }



        #endregion

        #region ExecuteDataReader
        /// <exclude />
        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void ExecuteDataReaderTestWithEmptyCommandText()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DbDataReader retVal = objDataAccess.ExecuteDataReader(_emptyCommandText);
        }

        /// <exclude />
        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void ExecuteDataReaderTestWithNullCommandText()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DbDataReader retVal = objDataAccess.ExecuteDataReader(_nullCommandText);
        }

        /// <exclude />
        [TestMethod()]
        public void ExecuteDataReaderTestWithValidCommandText()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            using (DbDataReader retVal = objDataAccess.ExecuteDataReader(_validCommandText))
            {
                Assert.IsTrue(retVal != null);
            }
        }

        /// <exclude />
        [TestMethod()]
        public void ExecuteDataReaderTestWithDMLCommandText()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            using (DbDataReader retVal = objDataAccess.ExecuteDataReader(_validDmlCommandText))
            {
                Assert.IsTrue(retVal != null);
            }
        }


        /// <exclude />
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExecuteDataReaderTestWithInValidCommandType()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            //CommandType.TableDirect is not valid command type for select command
            using (DbDataReader retVal = objDataAccess.ExecuteDataReader(_validCommandText, CommandType.TableDirect))
            {
                Assert.IsTrue(retVal != null);
            }
        }

        /// <exclude />
        [TestMethod()]
        public void ExecuteDataReaderTestWithValidCommandType()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();

            //object retVal = objDataAccess.ExecuteDataReader(_validCommandText, CommandType.Text);
            using (DbDataReader retVal = objDataAccess.ExecuteDataReader(_validCommandText, CommandType.Text))
            {
                Assert.IsTrue(retVal != null);
            }
        }

        /// <exclude />
        [TestMethod()]
        public void ExecuteDataReaderTestWithNullParameterList()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = null;

            using (DbDataReader retVal = objDataAccess.ExecuteDataReader(_validCommandText, CommandType.Text, dal))
            {
                Assert.IsTrue(retVal != null);
            }
        }

        /// <exclude />
        [TestMethod()]
        public void ExecuteDataReaderTestWithEmptyParameterList()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = new DalParameterList();

            using (DbDataReader retVal = objDataAccess.ExecuteDataReader(_validCommandText, CommandType.Text, dal))
            {
                Assert.IsTrue(retVal != null);
            }
        }

        /// <exclude />
        [TestMethod()]
        [ExpectedException(typeof(SqlException))]
        public void ExecuteDataReaderTestWithInvalidParameterList()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = new DalParameterList();
            dal.Add(new DalParameter()
            {
                ParameterName = "",
                ParameterDirection = ParameterDirection.Input
            });

            using (DbDataReader retVal = objDataAccess.ExecuteDataReader(_validCommandTextWithParameter, CommandType.Text, dal))
            {
                Assert.IsTrue(retVal != null);
            }
        }

        /// <exclude />
        [TestMethod()]
        [ExpectedException(typeof(SqlException))]
        public void ExecuteDataReaderTestWithoutInputParameterValue()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = new DalParameterList();


            //without Parameter value
            dal.Add(new DalParameter()
            {
                ParameterName = "@p1",
                ParameterDirection = ParameterDirection.Input
            });


            using (DbDataReader retVal = objDataAccess.ExecuteDataReader(_validCommandTextWithParameter, CommandType.Text, dal))
            {
                Assert.IsTrue(retVal != null);
            }

        }

        /// <exclude />
        [TestMethod()]
        [ExpectedException(typeof(SqlException))]
        public void ExecuteDataReaderTestWithInvalidParameterDirection()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = new DalParameterList();


            //without Parameter value
            dal.Add(new DalParameter()
            {
                ParameterName = "@p1",
                ParameterValue = 2,
                ParameterDirection = ParameterDirection.ReturnValue
            });


            using (DbDataReader retVal = objDataAccess.ExecuteDataReader(_validCommandTextWithParameter, CommandType.Text, dal))
            {
                Assert.IsTrue(retVal == null);
            }
        }

        /// <exclude />
        [TestMethod()]
        public void ExecuteDataReaderTestWithInvalidParameterDirectionOutput()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = new DalParameterList();


            //without Parameter value
            dal.Add(new DalParameter()
            {
                ParameterName = "@p1",
                ParameterValue = 2,
                ParameterDirection = ParameterDirection.Output
            });


            using (DbDataReader retVal = objDataAccess.ExecuteDataReader(_validCommandTextWithParameter, CommandType.Text, dal))
            {
                Assert.IsTrue(!retVal.HasRows);
            }
        }

        /// <exclude />
        [TestMethod()]
        public void ExecuteDataReaderTestWithValidParameter()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = new DalParameterList();


            //without Parameter value
            dal.Add(new DalParameter()
            {
                ParameterName = "@p1",
                ParameterValue = "2; select * from city",
                ParameterDirection = ParameterDirection.Input,
                ParameterType = SqlDbType.VarChar
            });

            using (DbDataReader retVal = objDataAccess.ExecuteDataReader(_validCommandTextWithParameter, CommandType.Text, dal))
            {
                Assert.IsTrue(retVal != null);
            }
        }


        #endregion

        #region ExecuteDataTable
        /// <exclude />
        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void ExecuteDataTableTestWithEmptyCommandText()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DataTable retVal = objDataAccess.ExecuteDataTable(_emptyCommandText);
        }

        /// <exclude />
        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void ExecuteDataTableTestWithNullCommandText()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DataTable retVal = objDataAccess.ExecuteDataTable(_nullCommandText);
        }

        /// <exclude />
        [TestMethod()]
        public void ExecuteDataTableTestWithValidCommandText()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DataTable retVal = objDataAccess.ExecuteDataTable(_validCommandText);
            Assert.IsTrue(retVal != null);
        }

        /// <exclude />
        [TestMethod()]
        public void ExecuteDataTableTestWithDMLCommandText()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DataTable retVal = objDataAccess.ExecuteDataTable(_validDmlCommandText);
            Assert.IsTrue(retVal == null);

        }


        /// <exclude />
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExecuteDataTableTestWithInValidCommandType()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            //CommandType.TableDirect is not valid command type for select command
            DataTable retVal = objDataAccess.ExecuteDataTable(_validCommandText, CommandType.TableDirect);
            Assert.IsTrue(retVal != null);

        }

        /// <exclude />
        [TestMethod()]
        public void ExecuteDataTableTestWithValidCommandType()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();

            DataTable retVal = objDataAccess.ExecuteDataTable(_validCommandText, CommandType.Text);
            Assert.IsTrue(retVal != null);

        }

        /// <exclude />
        [TestMethod()]
        public void ExecuteDataTableTestWithNullParameterList()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = null;

            DataTable retVal = objDataAccess.ExecuteDataTable(_validCommandText, CommandType.Text, dal);
            Assert.IsTrue(retVal != null);

        }

        /// <exclude />
        [TestMethod()]
        public void ExecuteDataTableTestWithEmptyParameterList()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = new DalParameterList();

            DataTable retVal = objDataAccess.ExecuteDataTable(_validCommandText, CommandType.Text, dal);
            Assert.IsTrue(retVal != null);

        }

        /// <exclude />
        [TestMethod()]
        [ExpectedException(typeof(SqlException))]
        public void ExecuteDataTableExecuteDataReaderTestWithInvalidParameterList()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = new DalParameterList();
            dal.Add(new DalParameter()
            {
                ParameterName = "",
                ParameterDirection = ParameterDirection.Input
            });

            DataTable retVal = objDataAccess.ExecuteDataTable(_validCommandTextWithParameter, CommandType.Text, dal);
            Assert.IsTrue(retVal != null);

        }

        /// <exclude />
        [TestMethod()]
        [ExpectedException(typeof(SqlException))]
        public void ExecuteDataTableTestWithoutInputParameterValue()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = new DalParameterList();


            //without Parameter value
            dal.Add(new DalParameter()
            {
                ParameterName = "@p1",
                ParameterDirection = ParameterDirection.Input
            });


            DataTable retVal = objDataAccess.ExecuteDataTable(_validCommandTextWithParameter, CommandType.Text, dal);
            Assert.IsTrue(retVal != null);


        }

        /// <exclude />
        [TestMethod()]
        [ExpectedException(typeof(SqlException))]
        public void ExecuteDataTableTestWithInvalidParameterDirection()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = new DalParameterList();


            //without Parameter value
            dal.Add(new DalParameter()
            {
                ParameterName = "@p1",
                ParameterValue = 2,
                ParameterDirection = ParameterDirection.ReturnValue
            });


            DataTable retVal = objDataAccess.ExecuteDataTable(_validCommandTextWithParameter, CommandType.Text, dal);
            Assert.IsTrue(retVal == null);

        }

        /// <exclude />
        [TestMethod()]
        public void ExecuteDataTableTestWithInvalidParameterDirectionOutput()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = new DalParameterList();


            //without Parameter value
            dal.Add(new DalParameter()
            {
                ParameterName = "@p1",
                ParameterValue = 2,
                ParameterDirection = ParameterDirection.Output
            });


            DataTable retVal = objDataAccess.ExecuteDataTable(_validCommandTextWithParameter, CommandType.Text, dal);
            Assert.IsTrue(retVal != null);

        }

        /// <exclude />
        [TestMethod()]
        public void ExecuteDataTableTestWithValidParameter()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = new DalParameterList();


            //without Parameter value
            dal.Add(new DalParameter()
            {
                ParameterName = "@p1",
                ParameterValue = 2,
                ParameterDirection = ParameterDirection.Input
            });

            DataTable retVal = objDataAccess.ExecuteDataTable(_validCommandTextWithParameter, CommandType.Text, dal);
            Assert.IsTrue(retVal != null);

        }


        [TestMethod()]
        public void ExecuteDataTableTestWithInvalidTableName()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = new DalParameterList();


            //without Parameter value
            dal.Add(new DalParameter()
            {
                ParameterName = "@p1",
                ParameterValue = 2,
                ParameterDirection = ParameterDirection.Input
            });
            string tableName = "";
            DataTable retVal = objDataAccess.ExecuteDataTable(_validCommandTextWithParameter, CommandType.Text, dal, tableName);
            Assert.IsTrue(retVal.TableName == "Table");

        }

        [TestMethod()]
        public void ExecuteDataTableTestWithNullTableName()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = new DalParameterList();


            //without Parameter value
            dal.Add(new DalParameter()
            {
                ParameterName = "@p1",
                ParameterValue = 2,
                ParameterDirection = ParameterDirection.Input
            });
            string tableName = null;
            DataTable retVal = objDataAccess.ExecuteDataTable(_validCommandTextWithParameter, CommandType.Text, dal, tableName);
            Assert.IsTrue(retVal.TableName == "Table");

        }

        [TestMethod()]
        public void ExecuteDataTableTestWithValidTableName()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = new DalParameterList();


            //without Parameter value
            dal.Add(new DalParameter()
            {
                ParameterName = "@p1",
                ParameterValue = 2,
                ParameterDirection = ParameterDirection.Input
            });
            string tableName = "CityData";
            DataTable retVal = objDataAccess.ExecuteDataTable(_validCommandTextWithParameter, CommandType.Text, dal, tableName);
            Assert.IsTrue(retVal.TableName == "CityData");

        }

        #endregion

        #region ExecuteDataSet
        /// <exclude />
        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void ExecuteDataSetTestWithEmptyCommandText()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DataSet retVal = objDataAccess.ExecuteDataSet(_emptyCommandText);
        }

        /// <exclude />
        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void ExecuteDataSetTestWithNullCommandText()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DataSet retVal = objDataAccess.ExecuteDataSet(_nullCommandText);
        }

        /// <exclude />
        [TestMethod()]
        public void ExecuteDataSetTestWithValidCommandText()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DataSet retVal = objDataAccess.ExecuteDataSet(_validCommandText);
            Assert.IsTrue(retVal != null);
        }

        /// <exclude />
        [TestMethod()]
        public void ExecuteDataSetTestWithDMLCommandText()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DataSet retVal = objDataAccess.ExecuteDataSet(_validDmlCommandText);
            Assert.IsTrue(retVal.Tables.Count == 0);

        }


        /// <exclude />
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExecuteDataSetTestWithInValidCommandType()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            //CommandType.TableDirect is not valid command type for select command
            DataSet retVal = objDataAccess.ExecuteDataSet(_validCommandText, CommandType.TableDirect);
            Assert.IsTrue(retVal != null);

        }

        /// <exclude />
        [TestMethod()]
        public void ExecuteDataSetTestWithValidCommandType()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();

            DataSet retVal = objDataAccess.ExecuteDataSet(_validCommandText, CommandType.Text);
            Assert.IsTrue(retVal != null);

        }

        /// <exclude />
        [TestMethod()]
        public void ExecuteDataSetTestWithNullParameterList()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = null;

            DataSet retVal = objDataAccess.ExecuteDataSet(_validCommandText, CommandType.Text, dal);
            Assert.IsTrue(retVal != null);

        }

        /// <exclude />
        [TestMethod()]
        public void ExecuteDataSetTestWithEmptyParameterList()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = new DalParameterList();

            DataSet retVal = objDataAccess.ExecuteDataSet(_validCommandText, CommandType.Text, dal);
            Assert.IsTrue(retVal != null);

        }

        /// <exclude />
        [TestMethod()]
        [ExpectedException(typeof(SqlException))]
        public void ExecuteDataSetExecuteDataReaderTestWithInvalidParameterList()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = new DalParameterList();
            dal.Add(new DalParameter()
            {
                ParameterName = "",
                ParameterDirection = ParameterDirection.Input
            });

            DataSet retVal = objDataAccess.ExecuteDataSet(_validCommandTextWithParameter, CommandType.Text, dal);
            Assert.IsTrue(retVal != null);

        }

        /// <exclude />
        [TestMethod()]
        [ExpectedException(typeof(SqlException))]
        public void ExecuteDataSetTestWithoutInputParameterValue()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = new DalParameterList();


            //without Parameter value
            dal.Add(new DalParameter()
            {
                ParameterName = "@p1",
                ParameterDirection = ParameterDirection.Input
            });


            DataSet retVal = objDataAccess.ExecuteDataSet(_validCommandTextWithParameter, CommandType.Text, dal);
            Assert.IsTrue(retVal != null);


        }

        /// <exclude />
        [TestMethod()]
        [ExpectedException(typeof(SqlException))]
        public void ExecuteDataSetTestWithInvalidParameterDirection()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = new DalParameterList();


            //without Parameter value
            dal.Add(new DalParameter()
            {
                ParameterName = "@p1",
                ParameterValue = 2,
                ParameterDirection = ParameterDirection.ReturnValue
            });


            DataSet retVal = objDataAccess.ExecuteDataSet(_validCommandTextWithParameter, CommandType.Text, dal);
            Assert.IsTrue(retVal == null);

        }

        /// <exclude />
        [TestMethod()]
        public void ExecuteDataSetTestWithInvalidParameterDirectionOutput()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = new DalParameterList();


            //without Parameter value
            dal.Add(new DalParameter()
            {
                ParameterName = "@p1",
                ParameterValue = 2,
                ParameterDirection = ParameterDirection.Output
            });


            DataSet retVal = objDataAccess.ExecuteDataSet(_validCommandTextWithParameter, CommandType.Text, dal);
            Assert.IsTrue(retVal != null);

        }

        /// <exclude />
        [TestMethod()]
        public void ExecuteDataSetTestWithValidParameter()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = new DalParameterList();


            //without Parameter value
            dal.Add(new DalParameter()
            {
                ParameterName = "@p1",
                ParameterValue = 2,
                ParameterDirection = ParameterDirection.Input
            });

            DataSet retVal = objDataAccess.ExecuteDataSet(_validCommandTextWithParameter, CommandType.Text, dal);
            Assert.IsTrue(retVal != null);

        }


        [TestMethod()]
        public void ExecuteDataSetTestWithInvalidTableNames()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = new DalParameterList();


            //without Parameter value
            dal.Add(new DalParameter()
            {
                ParameterName = "@p1",
                ParameterValue = 2,
                ParameterDirection = ParameterDirection.Input
            });
            string[] tableNames = new string[] { "" };
            DataSet retVal = objDataAccess.ExecuteDataSet(_validCommandTextWithParameter, CommandType.Text, dal, tableNames);
            Assert.IsTrue(retVal.Tables[0].TableName == "Table");

        }

        [TestMethod()]
        public void ExecuteDataSetTestWithNullTableNames()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = new DalParameterList();


            //without Parameter value
            dal.Add(new DalParameter()
            {
                ParameterName = "@p1",
                ParameterValue = 2,
                ParameterDirection = ParameterDirection.Input
            });
            string[] tableNames = null;
            DataSet retVal = objDataAccess.ExecuteDataSet(_validCommandTextWithParameter, CommandType.Text, dal, tableNames);
            Assert.IsTrue(retVal.Tables[0].TableName == "Table");

        }


        [TestMethod()]
        public void ExecuteDataSetTestWithValidTableNames()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = new DalParameterList();


            //without Parameter value
            dal.Add(new DalParameter()
            {
                ParameterName = "@p1",
                ParameterValue = 2,
                ParameterDirection = ParameterDirection.Input
            });
            string[] tableNames = new string[] { "CityData" };
            DataSet retVal = objDataAccess.ExecuteDataSet(_validCommandTextWithParameter, CommandType.Text, dal, tableNames);
            Assert.IsTrue(retVal.Tables[0].TableName == "CityData");

        }

        [TestMethod()]
        public void ExecuteDataSetTestWithInValidTableNamesList()
        {
            FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _validConnectionString);
            IDataAccess objDataAccess = objFactory.GetDataAccess();
            DalParameterList dal = new DalParameterList();


            //without Parameter value
            dal.Add(new DalParameter()
            {
                ParameterName = "@p1",
                ParameterValue = 2,
                ParameterDirection = ParameterDirection.Input
            });
            string[] tableNames = new string[] { "CityData", "sdasdas", "dasdasd", "dsadsad" };
            DataSet retVal = objDataAccess.ExecuteDataSet(_validCommandTextWithParameter, CommandType.Text, dal, tableNames);
            Assert.IsTrue(retVal.Tables[0].TableName == "CityData");

        }

        #endregion
    }
}