using System;

namespace Videospieldatenbank.Database
{
    public class UserDatabaseConnector : DatabaseConnector
    {
        private string _username;
        private string _password;
        private bool _isLoggedIn;

        /// <summary>
        /// Überprüft die Anmeldedaten eines Users.
        /// </summary>
        /// <param name="username">Der Name des Users.</param>
        /// <param name="password">Das Passwort des Users.</param>
        /// <returns>True, wenn der Login erfolgreich war.</returns>
        private bool CheckLogin(string username, string password)
        {
            if (!Exists(username)) return false;
            using (var command = MySqlConnection.CreateCommand())
            {
                command.CommandText = $"SELECT password FROM user WHERE name = '{username}'";
                using (var reader = command.ExecuteReader())
                {
                    return reader.Read() && password == reader[0] as string;
                }
            }
        }

        /// <summary>
        /// Meldet den User beim Server an.
        /// </summary>
        /// <param name="username">Name des Users.</param>
        /// <param name="password">Passwort des Users.</param>
        /// <returns>True, wenn Login erfolgreich war.</returns>
        public bool Login(string username, string password)
        {
            if (CheckLogin(username, password))
            {
                using (var command = MySqlConnection.CreateCommand())
                {
                    _username = username;
                    _password = password;
                    _isLoggedIn = true;

                    // Setzt Onlinestatus des Users auf true.
                    command.CommandText = $"UPDATE user SET online=true WHERE name = '{username}'";
                    command.ExecuteNonQuery();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Meldet einen User beim Server ab.
        /// </summary>
        /// <returns>True, wenn erfolgreich.</returns>
        public bool Logout()
        {
            if (!_isLoggedIn) return false;
            using (var command = MySqlConnection.CreateCommand())
            {
                // Setzt Onlinestatus des Users auf true.
                command.CommandText = $"UPDATE user SET online=false WHERE name = '{_username}'";
                command.ExecuteNonQuery();
                return true;
            }
        }

        /// <summary>
        /// Erstellt einen neuen User.
        /// </summary>
        /// <param name="username">Username des neuen Users.</param>
        /// <param name="password">Password des neuen Users.</param>
        /// <returns>True, wenn die Registrierung erfolgreich war.</returns>
        public bool Register(string username, string password)
        {
            if (!Exists(username))
            {
                using (var command = MySqlConnection.CreateCommand())
                {
                    command.CommandText = $"INSERT INTO user (name, password) VALUES ('{username}', '{password}')";
                    command.ExecuteNonQuery();
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gibt an ob ein User bereits existiert.
        /// </summary>
        /// <param name="username">Der zu überprüfende Username</param>
        /// <returns>True, wenn der User existiert.</returns>
        private bool Exists(string username)
        {
            using (var command = MySqlConnection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM user WHERE name = '{username}'";
                using (var reader = command.ExecuteReader())
                {
                    return reader.Read();
                }
            }
        }

        public bool SetProfilePicture(string username, string password, byte[] picture)
        {
            // TODO: Implement SetProfilePicture
            throw new NotImplementedException();
        }

        public void DeleteUser(string username, string password)
        {
            // TODO: Implement DeleteUser
            if (CheckLogin(username, password))
            {
                
            }
            throw new NotImplementedException();
        }
    }
}