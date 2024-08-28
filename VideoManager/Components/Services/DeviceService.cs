using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using VideoMatrix.Models;
using VideoMatrix.Data;

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

        public async Task AddDeviceAsync(Device device)
        {
            string query = "INSERT INTO Devices (Name, IpAddress, Status, ImageUrl, DeviceType) VALUES (@Name, @IpAddress, @Status, @ImageUrl, @DeviceType)";
            SqlParameter[] parameters = {
                new SqlParameter("@Name", device.Name),
                new SqlParameter("@IpAddress", device.IpAddress),
                new SqlParameter("@Status", device.Status),
                new SqlParameter("@ImageUrl", device.ImageUrl),
                new SqlParameter("@DeviceType", device.GetType().Name)
            };

            await _dataAccess.ExecuteNonQueryAsync(query, parameters);
        }

        public async Task UpdateDeviceAsync(Device device)
        {
            string query = "UPDATE Devices SET Name = @Name, IpAddress = @IpAddress, Status = @Status, ImageUrl = @ImageUrl WHERE Id = @Id";
            SqlParameter[] parameters = {
                new SqlParameter("@Name", device.Name),
                new SqlParameter("@IpAddress", device.IpAddress),
                new SqlParameter("@Status", device.Status),
                new SqlParameter("@ImageUrl", device.ImageUrl),
                new SqlParameter("@Id", device.Id)
            };

            await _dataAccess.ExecuteNonQueryAsync(query, parameters);
        }

        public async Task DeleteDeviceAsync(int id)
        {
            string query = "DELETE FROM Devices WHERE Id = @Id";
            SqlParameter[] parameters = {
                new SqlParameter("@Id", id)
            };

            await _dataAccess.ExecuteNonQueryAsync(query, parameters);
        }

        private Device MapToDevice(SqlDataReader reader)
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
