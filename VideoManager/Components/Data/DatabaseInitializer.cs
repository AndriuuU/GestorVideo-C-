using System;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace VideoMatrix.Data
{
    public class DatabaseInitializer
    {
        private readonly string _connectionString;

        public DatabaseInitializer(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task InitializeAsync()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Crear la base de datos si no existe
                var createDbCommand = new MySqlCommand("CREATE DATABASE IF NOT EXISTS VideoMatrix;", connection);
                await createDbCommand.ExecuteNonQueryAsync();

                // Usar la base de datos
                var useDbCommand = new MySqlCommand("USE VideoMatrix;", connection);
                await useDbCommand.ExecuteNonQueryAsync();

                // Crear la tabla Devices si no existe
                var createTableCommand = new MySqlCommand(@"
                    CREATE TABLE IF NOT EXISTS Devices (
                        Id INT PRIMARY KEY AUTO_INCREMENT,
                        Name VARCHAR(100) NOT NULL,
                        IpAddress VARCHAR(15) NOT NULL,
                        Status INT NOT NULL,
                        ImageUrl VARCHAR(255),
                        DeviceType VARCHAR(50) NOT NULL
                    );
                ", connection);
                await createTableCommand.ExecuteNonQueryAsync();

                // Crear la tabla Profiles si no existe
                var createProfilesTableCommand = new MySqlCommand(@"
                    CREATE TABLE IF NOT EXISTS Profiles (
                        Id INT PRIMARY KEY AUTO_INCREMENT,
                        Name VARCHAR(100) NOT NULL
                    );
                ", connection);
                await createProfilesTableCommand.ExecuteNonQueryAsync();

                // Insertar datos de prueba en Devices
                var checkDevicesCommand = new MySqlCommand("SELECT COUNT(*) FROM Devices;", connection);
                var devicesCount = Convert.ToInt32(await checkDevicesCommand.ExecuteScalarAsync());

                if (devicesCount == 0)
                {
                    var seedDevicesCommand = new MySqlCommand(@"
                        INSERT INTO Devices (Name, IpAddress, Status, ImageUrl, DeviceType) VALUES
                        ('Transmitter 1', '192.168.1.1', 1, 'https://via.placeholder.com/150', 'Transmitter'),
                        ('Transmitter 2', '192.168.1.2', 2, 'https://via.placeholder.com/150', 'Transmitter'),
                        ('Receiver 1', '192.168.1.3', 0, 'https://via.placeholder.com/150', 'Receiver'),
                        ('Receiver 2', '192.168.1.4', 1, 'https://via.placeholder.com/150', 'Receiver');
                    ", connection);
                    await seedDevicesCommand.ExecuteNonQueryAsync();
                }

                // Insertar datos de prueba en Profiles
                var checkProfilesCommand = new MySqlCommand("SELECT COUNT(*) FROM Profiles;", connection);
                var profilesCount = Convert.ToInt32(await checkProfilesCommand.ExecuteScalarAsync());

                if (profilesCount == 0)
                {
                    var seedProfilesCommand = new MySqlCommand(@"
                        INSERT INTO Profiles (Name) VALUES ('Default Profile');
                    ", connection);
                    await seedProfilesCommand.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
