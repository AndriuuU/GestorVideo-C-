using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VideoMatrix.Models;
using VideoMatrix.Data;
using MySql.Data.MySqlClient;

namespace VideoMatrix.Services
{
    public class DeviceService
    {
        private readonly DataAccess _dataAccess;
        private readonly ProfileService _profileService;
        private readonly TransmitterService _transmitterService;

        public DeviceService(string connectionString)
        {
            _dataAccess = new DataAccess(connectionString);
            _profileService = new ProfileService(connectionString);
            _transmitterService = new TransmitterService(connectionString);
        }

        public async Task<List<Device>> GetAllDevicesAsync()
        {
            string query = "SELECT * FROM Devices";
            return await _dataAccess.ExecuteQueryAsync(query, reader => MapToDevice(reader));
        }

        public async Task<List<Transmitter>> GetTransmittersAsync()
        {
            return await _transmitterService.GetTransmittersAsync(); // Utiliza el método de TransmitterService
        }

        public async Task<List<Receiver>> GetReceiversAsync()
        {
            string query = "SELECT * FROM Devices WHERE DeviceType = 'Receiver'";
            return await _dataAccess.ExecuteQueryAsync(query, reader => (Receiver)MapToDevice(reader));
        }

        public async Task<List<Profil>> GetProfilesAsync()
        {
            return await _profileService.GetProfilesAsync();
        }

        public async Task CreateProfileAsync(Profil profil)
        {
            await _profileService.AddProfileAsync(profil);
        }
 public async Task<List<Transmitter>> GetTransmittersByIdsAsync(IEnumerable<int> transmitterIds)
        {
            if (transmitterIds == null || !transmitterIds.Any())
                return new List<Transmitter>();

            string ids = string.Join(",", transmitterIds);
            string query = $"SELECT * FROM Transmitters WHERE Id IN ({ids})";

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

            device.Id = Convert.ToInt32(reader["Id"]);
            device.Name = reader["Name"].ToString();
            device.IpAddress = reader["IpAddress"].ToString();
            device.Status = (DeviceStatus)Enum.Parse(typeof(DeviceStatus), reader["Status"].ToString());
            device.ImageUrl = reader["ImageUrl"].ToString();

            return device;
        }
    }
}
