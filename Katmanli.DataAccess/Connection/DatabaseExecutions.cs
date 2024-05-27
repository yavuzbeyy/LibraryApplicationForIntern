using Katmanli.DataAccess.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Newtonsoft.Json;
using System.Collections.Generic;
using Katmanli.DataAccess.DTOs;

namespace Katmanli.DataAccess.Connection
{
    public class DatabaseExecutions : IDatabaseExecutions
    {

        private readonly string _connectionString;
        private readonly IConfiguration _configuration;

        public DatabaseExecutions(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("DatabaseConnection");
        }


        public string ExecuteQuery(string storedProcedureName, ParameterList parameters)
        {
            List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, sqlConnection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    foreach (var parameter in parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Name, parameter.Value);
                    }

                    sqlConnection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Dictionary<string, object> row = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string columnName = reader.GetName(i);
                                object value = reader.IsDBNull(i) ? null : reader.GetValue(i);
                                row[columnName] = value;
                            }
                            results.Add(row);
                        }
                    }
                }
            }

            // JSON dönüşümü
            string jsonResult = JsonConvert.SerializeObject(results);
            return jsonResult;
        }

        public int ExecuteDeleteQuery(string storedProcedureName, ParameterList parameters)
        {
          //  List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, sqlConnection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    foreach (var parameter in parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Name, parameter.Value);
                    }

                    sqlConnection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    return rowsAffected;
                }
            }
        }

    }
}

