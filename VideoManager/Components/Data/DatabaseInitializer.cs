using System;
using System.Data.SqlClient;
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
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Crear la base de datos si no existe
                var createDbCommand = new SqlCommand("IF DB_ID('VideoMatrix') IS NULL CREATE DATABASE VideoMatrix;", connection);
                await createDbCommand.ExecuteNonQueryAsync();

                // Usar la base de datos
                var useDbCommand = new SqlCommand("USE VideoMatrix;", connection);
                await useDbCommand.ExecuteNonQueryAsync();

                // Crear la tabla Devices si no existe
                var createTableCommand = new SqlCommand(@"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Devices' AND xtype='U')
                    CREATE TABLE Devices (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        Name NVARCHAR(100) NOT NULL,
                        IpAddress NVARCHAR(15) NOT NULL,
                        Status INT NOT NULL,
                        ImageUrl NVARCHAR(255),
                        DeviceType NVARCHAR(50) NOT NULL
                    );
                ", connection);
                await createTableCommand.ExecuteNonQueryAsync();

                // Crear la tabla Profiles si no existe
                var createProfilesTableCommand = new SqlCommand(@"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Profiles' AND xtype='U')
                    CREATE TABLE Profiles (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        Name NVARCHAR(100) NOT NULL
                    );
                ", connection);
                await createProfilesTableCommand.ExecuteNonQueryAsync();

                // Insertar datos de prueba
                /*var seedDataCommand = new SqlCommand(@"
                    IF NOT EXISTS (SELECT * FROM Devices)
                    BEGIN
                        INSERT INTO Devices (Name, IpAddress, Status, ImageUrl, DeviceType) VALUES
                        ('Transmitter 1', '192.168.1.1', 1, 'https://via.placeholder.com/150', 'Transmitter'),
                        ('Transmitter 2', '192.168.1.2', 2, 'https://via.placeholder.com/150', 'Transmitter'),
                        ('Receiver 1', '192.168.1.3', 0, 'https://via.placeholder.com/150', 'Receiver'),
                        ('Receiver 2', '192.168.1.4', 1, 'https://via.placeholder.com/150', 'Receiver');
                    END;

                    IF NOT EXISTS (SELECT * FROM Profiles)
                    BEGIN
                        INSERT INTO Profiles (Name) VALUES ('Default Profile');
                    END;
                ", connection);
                await seedDataCommand.ExecuteNonQueryAsync();
                */
            }
        }
    }
}
