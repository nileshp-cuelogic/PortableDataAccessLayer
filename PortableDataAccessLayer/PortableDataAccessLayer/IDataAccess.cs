// ***********************************************************************
// Assembly         : PortableDataAccessLayer
// Author           : Admin
// Created          : 04-19-2016
//
// Last Modified By : Admin
// Last Modified On : 04-20-2016
// ***********************************************************************
// <copyright file="IDataAccess.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Data;
using System.Data.Common;

namespace PortableDataAccessLayer
{

    /// <summary>
    /// Interface IDataAccess
    /// </summary>
    public interface IDataAccess
    {

        /// <summary>
        /// Executes the Sql Command or Stored Procedure and returns a single value. 
        /// (First row's first cell value, if more than one row and column is returned.
        /// </summary>
        /// <param name="commandText">Sql Command or Stored Procedure name</param>
        /// <param name="commandType">Type of command (i.e. Sql Command/ Stored Procedure name/ Table Direct)</param>
        /// <returns>System.Object.</returns>
        object ExecuteScaler(string commandText);


        /// <summary>
        /// Executes the Sql Command or Stored Procedure and returns a single value. 
        /// (First row's first cell value, if more than one row and column is returned.
        /// </summary>
        /// <param name="commandText">Sql Command or Stored Procedure name</param>
        /// <param name="commandType">Type of command (i.e. Sql Command/ Stored Procedure name/ Table Direct)</param>
        /// <returns>System.Object.</returns>
        object ExecuteScaler(string commandText, CommandType commandType);


        /// <summary>
        /// Executes the Sql Command or Stored Procedure and returns a single value. 
        /// (First row's first cell value, if more than one row and column is returned.
        /// </summary>
        /// <param name="commandText">Sql Command or Stored Procedure name</param>
        /// <param name="commandType">Type of command (i.e. Sql Command/ Stored Procedure name/ Table Direct)</param>
        /// <param name="dalParameterList">Parameter List to be associated with the Command or Stored Procedure.</param>
        /// <returns>System.Object.</returns>
        object ExecuteScaler(string commandText, CommandType commandType, DalParameterList dalParameterList);


        /// <summary>
        /// Executes Sql Command or Stored procedure and returns number of rows affected.
        /// </summary>
        /// <param name="commandText">Sql Command or Stored Procedure name</param>
        /// <param name="commandType">Type of command (i.e. Sql Command/ Stored Procedure name/ Table Direct)</param>
        /// <returns>Number of rows affected.</returns>
        int ExecuteNonQuery(string commandText);


        /// <summary>
        /// Executes Sql Command or Stored procedure and returns number of rows affected.
        /// </summary>
        /// <param name="commandText">Sql Command or Stored Procedure name</param>
        /// <param name="commandType">Type of command (i.e. Sql Command/ Stored Procedure name/ Table Direct)</param>
        /// <returns>Number of rows affected.</returns>
        int ExecuteNonQuery(string commandText, CommandType commandType);


        /// <summary>
        /// Executes Sql Command or Stored procedure and returns number of rows affected.
        /// </summary>
        /// <param name="commandText">Sql Command or Stored Procedure name</param>
        /// <param name="commandType">Type of command (i.e. Sql Command/ Stored Procedure name/ Table Direct)</param>
        /// /// <param name="dalParameterList">Parameter List to be associated with the Command or Stored Procedure.</param>
        /// <returns>Number of rows affected.</returns>
        int ExecuteNonQuery(string commandText, CommandType commandType, DalParameterList dalParameterList);


        /// <summary>
        /// Executes the Sql Command or Stored Procedure and returns the DataReader.
        /// Call this method in using block to close the open connection automatically.
        /// </summary>
        /// <param name="commandText">Sql Command or Stored Procedure Name</param>        
        /// <returns>DbDataReader.</returns>
        DbDataReader ExecuteDataReader(string commandText);


        /// <summary>
        /// Executes the Sql Command or Stored Procedure and returns the DataReader.
        /// Call this method in using block to close the open connection automatically.
        /// eg. 
        /// </summary>
        /// <param name="commandText">Sql Command or Stored Procedure Name</param>        
        /// <param name="commandType">Type of command (i.e. Sql Command/ Stored Procedure name/ Table Direct)</param>
        /// <returns>DbDataReader</returns>
        DbDataReader ExecuteDataReader(string commandText, CommandType commandType);


        /// <summary>
        /// Executes the Sql Command or Stored Procedure and returns the DataReader.
        /// Call this method in using block to close the open connection automatically.
        /// </summary>
        /// <param name="commandText">Sql Command or Stored Procedure Name</param>        
        /// <param name="con">Database Connection object (DBHelper.GetConnObject() may be used)</param>
        /// <param name="commandType">Type of command (i.e. Sql Command/ Stored Procedure name/ Table Direct)</param>
        /// <param name="dalParameterList">Parameter List to be associated with the Command or Stored Procedure.</param>
        /// <returns>DbDataReader.</returns>
        DbDataReader ExecuteDataReader(string commandText, CommandType commandType, DalParameterList dalParameterList);


        /// <summary>
        /// Executes the Sql Command or Stored Procedure and return result set in the form of DataTable.
        /// </summary>
        /// <param name="commandText">Sql Command or Stored Procedure name</param>
        /// <param name="tableName">Table name</param>
        /// <returns>Result in the form of DataTable</returns>
        DataTable ExecuteDataTable(string commandText, string tableName = null);


        /// <summary>
        /// Executes the Sql Command or Stored Procedure and return result set in the form of DataTable.
        /// </summary>
        /// <param name="commandText">Sql Command or Stored Procedure name</param>
        /// <param name="tableName">Table name</param>
        /// <param name="commandType">Type of command (i.e. Sql Command/ Stored Procedure name/ Table Direct)</param>
        /// <returns>Result in the form of DataTable</returns>
        DataTable ExecuteDataTable(string commandText, CommandType commandType, string tableName = null);


        /// <summary>
        /// Executes the Sql Command or Stored Procedure and return result set in the form of DataTable.
        /// </summary>
        /// <param name="commandText">Sql Command or Stored Procedure name</param>
        /// <param name="tableName">Table name</param>
        /// <param name="dalParameterList">Parameter List to be associated with the Command or Stored Procedure.</param>
        /// <param name="commandType">Type of command (i.e. Sql Command/ Stored Procedure name/ Table Direct)</param>
        /// <returns>Result in the form of DataTable</returns>
        DataTable ExecuteDataTable(string commandText, CommandType commandType, DalParameterList dalParameterList, string tableName = null);


        /// <summary>
        /// Executes the Sql Command or Stored Procedure and return result set in the form of DataSet.
        /// </summary>
        /// <param name="commandText">Sql Command or Stored Procedure name</param>
        /// <param name="tableNames">Table Names</param>
        /// <returns>Result in the form of DataSet</returns>
        DataSet ExecuteDataSet(string commandText, string[] tableNames = null);


        /// <summary>
        /// Executes the Sql Command or Stored Procedure and return result set in the form of DataSet.
        /// </summary>
        /// <param name="commandText">Sql Command or Stored Procedure name</param>
        /// <param name="tableNames">Table Names</param>
        /// <param name="commandType">Type of command (i.e. Sql Command/ Stored Procedure name/ Table Direct)</param>
        /// <returns>Result in the form of DataSet</returns>
        DataSet ExecuteDataSet(string commandText, CommandType commandType, string[] tableNames = null);


        /// <summary>
        /// Executes the Sql Command or Stored Procedure and return result set in the form of DataSet.
        /// </summary>
        /// <param name="commandText">Sql Command or Stored Procedure name</param>
        /// <param name="tableNames">Table Names</param>
        /// <param name="commandType">Type of command (i.e. Sql Command/ Stored Procedure name/ Table Direct)</param>
        /// <param name="dalParameterList">Parameter List to be associated with the Command or Stored Procedure.</param>
        /// <returns>Result in the form of DataSet</returns>
        DataSet ExecuteDataSet(string commandText, CommandType commandType, DalParameterList dalParameterList, string[] tableNames = null);
    }
}
