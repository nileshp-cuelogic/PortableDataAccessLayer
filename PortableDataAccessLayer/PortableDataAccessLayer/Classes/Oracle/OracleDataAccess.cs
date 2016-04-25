using System;
using System.Data;
using System.Data.Common;

namespace PortableDataAccessLayer
{

    internal class OracleDataAccess : IDataAccess
    {
        public DbDataReader ExecuteDataReader(string commandText)
        {
            throw new NotImplementedException();
        }

        public DbDataReader ExecuteDataReader(string commandText, CommandType commandType)
        {
            throw new NotImplementedException();
        }

        public DbDataReader ExecuteDataReader(string commandText, CommandType commandType, DalParameterList dalParameterList)
        {
            throw new NotImplementedException();
        }

        public DataSet ExecuteDataSet(string commandText, string[] TableNames = null)
        {
            throw new NotImplementedException();
        }

        public DataSet ExecuteDataSet(string commandText, CommandType commandType, string[] TableNames = null)
        {
            throw new NotImplementedException();
        }

        public DataSet ExecuteDataSet(string commandText, CommandType commandType, DalParameterList dalParameterList, string[] TableNames = null)
        {
            throw new NotImplementedException();
        }

        public DataTable ExecuteDataTable(string commandText, string TableName = null)
        {
            throw new NotImplementedException();
        }

        public DataTable ExecuteDataTable(string commandText, CommandType commandType, string TableName = null)
        {
            throw new NotImplementedException();
        }

        public DataTable ExecuteDataTable(string commandText, CommandType commandType, DalParameterList dalParameterList, string TableName = null)
        {
            throw new NotImplementedException();
        }

        public int ExecuteNonQuery(string commandText)
        {
            throw new NotImplementedException();
        }

        public int ExecuteNonQuery(string commandText, CommandType commandType)
        {
            throw new NotImplementedException();
        }

        public int ExecuteNonQuery(string commandText, CommandType commandType, DalParameterList dalParameterList)
        {
            throw new NotImplementedException();
        }

        public object ExecuteScaler(string commandText)
        {
            throw new NotImplementedException();
        }

        public object ExecuteScaler(string commandText, CommandType commandType)
        {
            throw new NotImplementedException();
        }

        public object ExecuteScaler(string commandText, CommandType commandType, DalParameterList dalParameterList)
        {
            throw new NotImplementedException();
        }
    }
}
