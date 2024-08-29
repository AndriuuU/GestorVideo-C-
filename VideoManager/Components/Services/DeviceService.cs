using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VideoMatrix.Models;
using VideoMatrix.Data;
using MySql.Data.MySqlClient; // Asegúrate de que esta línea esté presente

namespace VideoMatrix.Services
{
    public class DeviceService
    {
        private readonly DataAccess _dataAccess;

        public DeviceService(string connectionString)
        {
            _dataAccess = new DataAccess(connectionString);
        }

        public async Task<List<Device>> GetAllDevicesAsync()
        {
            string query = "SELECT * FROM Devices";
            return await _dataAccess.ExecuteQueryAsync(query, reader => MapToDevice(reader));
        }

        public async Task<List<Transmitter>> GetTransmittersAsync()
        {
            string query = "SELECT * FROM Devices WHERE DeviceType = 'Transmitter'";
            return await _dataAccess.ExecuteQueryAsync(query, reader => (Transmitter)MapToDevice(reader));
        }

        public async Task<List<Receiver>> GetReceiversAsync()
        {
            string query = "SELECT * FROM Devices WHERE DeviceType = 'Receiver'";
            return await _dataAccess.ExecuteQueryAsync(query, reader => (Receiver)MapToDevice(reader));
        }

        private Device MapToDevice(MySqlDataReader reader)
        {
            var deviceType = reader["DeviceType"].ToString();
            Device device;

            if (deviceType == "Transmitter")
            {
                device = new Transmitter();
            }
            else
            {
                device = new Receiver();
            }

            device.Id = (int)reader["Id"];
            device.Name = reader["Name"].ToString();
            device.IpAddress = reader["IpAddress"].ToString();
            device.Status = (DeviceStatus)Enum.Parse(typeof(DeviceStatus), reader["Status"].ToString());
            device.ImageUrl = reader["ImageUrl"].ToString();

            return device;
        }
    }
}
