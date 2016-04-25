// ***********************************************************************
// Assembly         : PortableDataAccessLayer
// Author           : Admin
// Created          : 04-19-2016
//
// Last Modified By : Admin
// Last Modified On : 04-19-2016
// ***********************************************************************
// <copyright file="DalParameter.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Data;

namespace PortableDataAccessLayer
{
    /// <summary>
    /// Class DalParameter.
    /// </summary>
    public class DalParameter
    {
        /// <summary>
        /// Gets or sets the name of the parameter.
        /// </summary>
        /// <value>The name of the parameter.</value>
        public string ParameterName { get; set; }

        /// <summary>
        /// Gets or sets the type of the parameter.
        /// </summary>
        /// <value>The type of the parameter.</value>
        public SqlDbType ParameterType { get; set; }

        /// <summary>
        /// Gets or sets the parameter direction.
        /// </summary>
        /// <value>The parameter direction.</value>
        public ParameterDirection ParameterDirection { get; set; }

        /// <summary>
        /// Gets or sets the parameter value.
        /// </summary>
        /// <value>The parameter value.</value>
        public object ParameterValue { get; set; }

        /// <summary>
        /// Gets or sets the size of the parameter.
        /// </summary>
        /// <value>The size of the parameter.</value>
        public int ParameterSize { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DalParameter"/> class.
        /// </summary>
        public DalParameter()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DalParameter"/> class.
        /// </summary>
        /// <param name="_ParameterName">Name of the _ parameter.</param>
        /// <param name="_ParameterType">Type of the _ parameter.</param>
        /// <param name="_ParameterDirection">The _ parameter direction.</param>
        /// <param name="_ParameterValue">The _ parameter value.</param>
        public DalParameter(string _ParameterName, SqlDbType _ParameterType, ParameterDirection _ParameterDirection = ParameterDirection.Input, object _ParameterValue = null)
        {
            this.ParameterName = _ParameterName;
            this.ParameterType = _ParameterType;
            this.ParameterDirection = _ParameterDirection;
            this.ParameterValue = _ParameterValue;
        }
    }

    
}
