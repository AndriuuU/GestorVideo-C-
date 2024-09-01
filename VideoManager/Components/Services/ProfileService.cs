using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VideoMatrix.Data;
using MySql.Data.MySqlClient;
using VideoMatrix.Models;

namespace VideoMatrix.Services
{
    public class ProfileService
    {
        private readonly DataAccess _dataAccess;

        public ProfileService(string connectionString)
        {
            _dataAccess = new DataAccess(connectionString);
        }

        public async Task<List<Profil>> GetProfilesAsync()  
        {
            string query = "SELECT * FROM Profiles";
            return await _dataAccess.ExecuteQueryAsync(query, MapToProfile);
        }

        public async Task<Profil> GetProfileByIdAsync(int id)  
        {
            string query = "SELECT * FROM Profiles WHERE Id = @Id";
            var parameters = new MySqlParameter[] { new MySqlParameter("@Id", id) };
            return await _dataAccess.ExecuteQuerySingleAsync(query, parameters, MapToProfile);
        }

        public async Task AddProfileAsync(Profil profile)  // Cambia Profile a Profil
        {
            string query = "INSERT INTO Profiles (Name) VALUES (@Name)";
            var parameters = new MySqlParameter[] { new MySqlParameter("@Name", profile.Name) };
            await _dataAccess.ExecuteNonQueryAsync(query, parameters);
        }

        private Profil MapToProfile(MySqlDataReader reader)  
        {
            return new Profil
            {
                Id = Convert.ToInt32(reader["Id"]),
                Name = reader["Name"].ToString(),
                TransmitterIds = new List<int>() 
            };
        }
    }
}
