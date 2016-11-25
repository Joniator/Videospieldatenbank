using System;
using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;

namespace Videospieldatenbank
{
    internal class DatabaseInteraction
    {
        private readonly MySqlConnection _connection;

        public DatabaseInteraction(string server, int port, string databaseName, string userId, string password)
        {
            try
            {   
                var connectionString = $"Server={server};" +
                                                   $"port={port};" +
                                                   $"Database={databaseName};" +
                                                   $"Uid={userId};"+
                                                   $"password={password}";

                _connection = new MySqlConnection(connectionString);
                _connection.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                Environment.Exit(1);
            }
        }

        public bool Connected => _connection.State == ConnectionState.Open;
    }
}