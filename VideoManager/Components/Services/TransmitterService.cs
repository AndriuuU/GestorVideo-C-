using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VideoMatrix.Models;
using VideoMatrix.Data;
using MySql.Data.MySqlClient;

namespace VideoMatrix.Services
{
    public class TransmitterService
    {
        private readonly DataAccess _dataAccess;

        public TransmitterService(string connectionString)
        {
            _dataAccess = new DataAccess(connectionString);
        }

        public async Task<List<Transmitter>> GetTransmittersAsync()
        {
            string query = "SELECT * FROM Devices WHERE DeviceType = 'Transmitter'";
            return await _dataAccess.ExecuteQueryAsync(query, MapToTransmitter);
        }

        private Transmitter MapToTransmitter(MySqlDataReader reader)
        {
            return new Transmitter
            {
                Id = Convert.ToInt32(reader["Id"]),
                Name = reader["Name"].ToString(),
                IpAddress = reader["IpAddress"].ToString(),
                Status = (DeviceStatus)Enum.Parse(typeof(DeviceStatus), reader["Status"].ToString()),
                ImageUrl = reader["ImageUrl"].ToString()
            };
        }
    }
}
