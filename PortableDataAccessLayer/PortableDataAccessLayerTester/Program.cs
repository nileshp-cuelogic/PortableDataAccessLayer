using PortableDataAccessLayer;
using System;
using System.Data;
using System.Data.Common;

namespace PortableDataAccessLayerTester
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string _connectionString = "Data Source=TEST-PC;Initial Catalog=kantarPractice;Integrated Security=False;User ID=sa;Password=pa$$word;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

                FactoryDataAccess objFactory = new FactoryDataAccess(Provider.Sql, _connectionString);

                IDataAccess objDataAccess = objFactory.GetDataAccess();
                DalParameterList dal = new DalParameterList();
                dal.Add(new DalParameter()
                {
                    ParameterName = "@cityid",
                    ParameterDirection = ParameterDirection.Input,
                    ParameterType = SqlDbType.Int,
                    ParameterValue = 2

                });

                //string[] tableNames = new string[] { "", "", "", "", "", "" };

                //DataSet obj = objDataAccess.ExecuteDataSet("[usp_GetCitiesWithParameter]", tableNames);
                //Console.WriteLine( );
                using (DbDataReader reader = objDataAccess.ExecuteDataReader("[usp_GetCitiesWithParameter]", CommandType.StoredProcedure, dal))
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("{0}", reader.GetString(0));
                        }
                    }
                }

                using (DbDataReader reader = objDataAccess.ExecuteDataReader("[usp_GetCitiesWithParameter]", CommandType.StoredProcedure, dal))
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("{0}", reader.GetString(0));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetBaseException());
            }
            Console.ReadKey(true);
        }
    }
}
