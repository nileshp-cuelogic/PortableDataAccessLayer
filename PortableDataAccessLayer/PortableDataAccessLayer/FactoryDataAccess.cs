// ***********************************************************************
// Assembly         : PortableDataAccessLayer
// Author           : Admin
// Created          : 04-19-2016
//
// Last Modified By : Admin
// Last Modified On : 04-20-2016
// ***********************************************************************
// <copyright file="FactoryDataAccess.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Configuration;

namespace PortableDataAccessLayer
{
    /// <summary>
    /// Class FactoryDataAccess.
    /// </summary>
    public class FactoryDataAccess
    {
        /// <summary>
        /// The _default provider
        /// </summary>
        Provider _defaultProvider = Provider.Sql;
        /// <summary>
        /// The _connection string
        /// </summary>
        private string _connectionString = "";
        /// <summary>
        /// Initializes a new instance of the <see cref="FactoryDataAccess"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <exception cref="System.Exception">Connection string should not be empty</exception>
        public FactoryDataAccess(Provider provider, string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString) || string.IsNullOrWhiteSpace(connectionString))
                throw new Exception("Connection string should not be empty");
            if (provider == Provider.Sql)
            {
                try
                {
                    var connBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder(connectionString);
                }
                catch (Exception ex)
	            {
                    throw new Exception("Invalid Connection string \n" + ex.Message);
                }
                
            }
            _defaultProvider = provider;
            _connectionString = connectionString;
            
        }
        /// <summary>
        /// Gets the data access.
        /// </summary>
        /// <returns>IDataAccess.</returns>
        public IDataAccess GetDataAccess()
        {
            IDataAccess iDataAccess = null;
            switch (_defaultProvider)
            {
                case Provider.Sql:
                    iDataAccess = new SqlDataAccess(_connectionString);
                    break;
                case Provider.Oracle:
                    iDataAccess = new OracleDataAccess();
                    break;
            }
            return iDataAccess;
        }
    }
}
