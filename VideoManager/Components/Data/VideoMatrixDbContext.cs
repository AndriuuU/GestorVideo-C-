using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using VideoMatrix.Models;

namespace VideoMatrix.Data
{
    public class DataAccess
    {
        private readonly string _connectionString;

        public DataAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        // Método para ejecutar consultas que retornan múltiples filas (SELECT)
        public async Task<List<T>> ExecuteQueryAsync<T>(string query, Func<SqlDataReader, T> mapFunction)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    var results = new List<T>();
                    while (await reader.ReadAsync())
                    {
                        results.Add(mapFunction(reader));
                    }
                    return results;
                }
            }
        }

        // Método para ejecutar comandos INSERT, UPDATE, DELETE
        public async Task ExecuteNonQueryAsync(string query, SqlParameter[]? parameters = null)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
