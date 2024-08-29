using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VideoMatrix.Data
{
    public class DataAccess
    {
        private readonly string _connectionString;

        public DataAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<T>> ExecuteQueryAsync<T>(string query, Func<MySqlDataReader, T> mapFunc)
        {
            var result = new List<T>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = (MySqlDataReader)await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(mapFunc(reader));
                        }
                    }
                }
            }

            return result;
        }

        public async Task ExecuteNonQueryAsync(string query, MySqlParameter[] parameters)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddRange(parameters);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
