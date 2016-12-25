using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Videospieldatenbank.Database
{
    public class UserDatabaseConnector : DatabaseConnector
    {
        private bool _isLoggedIn;
        private string _password;
        private string _username;

        /// <summary>
        /// Gibt eine Liste mit allen Freunden des Users zurück.
        /// </summary>
        /// <returns></returns>
        public List<string> GetFriendsList()
        {
            List<string> list = new List<string>();
            if (_isLoggedIn)
            {
                using (MySqlCommand command = MySqlConnection.CreateCommand())
                {
                    command.CommandText = $"SELECT friend_id FROM friends WHERE user_ID ='{_username}'";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read()) list.Add(GetUsername(reader.GetString(0)));
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Ermittelt den Namen des Users anhand seiner ID.
        /// </summary>
        /// <param name="id">ID des Users.</param>
        /// <returns>Name des Users.</returns>
        private string GetUsername(string id)
        {
            if (_isLoggedIn)
            {
                using (MySqlCommand command = MySqlConnection.CreateCommand())
                {
                    command.CommandText = $"SELECT name FROM user WHERE id='{id}'";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            return reader.GetString(0);
                    }
                }
            }
            return "";
        }

        /// <summary>
        ///     Überprüft die Anmeldedaten eines Users.
        /// </summary>
        /// <param name="username">Der Name des Users.</param>
        /// <param name="password">Das Passwort des Users.</param>
        /// <returns>True, wenn der Login erfolgreich war.</returns>
        private bool CheckLogin(string username, string password)
        {
            if (!Exists(username)) return false;
            using (MySqlCommand command = MySqlConnection.CreateCommand())
            {
                command.CommandText = $"SELECT password FROM user WHERE name = '{username}'";
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    return reader.Read() && password == reader[0] as string;
                }
            }
        }

        /// <summary>
        ///     Meldet den User beim Server an.
        /// </summary>
        /// <param name="username">Name des Users.</param>
        /// <param name="password">Passwort des Users.</param>
        /// <returns>True, wenn Login erfolgreich war.</returns>
        public bool Login(string username, string password)
        {
            if (CheckLogin(username, password))
            {
                using (MySqlCommand command = MySqlConnection.CreateCommand())
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
        ///     Meldet einen User beim Server ab.
        /// </summary>
        /// <returns>True, wenn erfolgreich.</returns>
        public bool Logout()
        {
            if (!_isLoggedIn) return false;
            using (MySqlCommand command = MySqlConnection.CreateCommand())
            {
                // Setzt Onlinestatus des Users auf false.
                command.CommandText = $"UPDATE user SET online=false WHERE name = '{_username}'";
                command.ExecuteNonQuery();
                return true;
            }
        }

        /// <summary>
        ///     Erstellt einen neuen User.
        /// </summary>
        /// <param name="username">Username des neuen Users.</param>
        /// <param name="password">Password des neuen Users.</param>
        /// <returns>True, wenn die Registrierung erfolgreich war.</returns>
        public bool Register(string username, string password)
        {
            if (!Exists(username))
            {
                using (MySqlCommand command = MySqlConnection.CreateCommand())
                {
                    command.CommandText = $"INSERT INTO user (name, password) VALUES ('{username}', '{password}')";
                    command.ExecuteNonQuery();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///     Gibt an ob ein User bereits existiert.
        /// </summary>
        /// <param name="username">Der zu überprüfende Username</param>
        /// <returns>True, wenn der User existiert.</returns>
        private bool Exists(string username)
        {
            using (MySqlCommand command = MySqlConnection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM user WHERE name = '{username}'";
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    return reader.Read();
                }
            }
        }

        /// <summary>
        ///     Setzt das Profilbild des Users
        /// </summary>
        /// <param name="picture">Array mit Image-Dateien</param>
        /// <returns>True, wenn erfolgreich</returns>
        public bool SetProfilePicture(byte[] picture)
        {
            if (Exists(_username))
            {
                using (MySqlCommand command = MySqlConnection.CreateCommand())
                {
                    command.CommandText = $"UPDATE user SET picture=?image WHERE name='{_username}'";
                    command.Parameters.Add(new MySqlParameter("?image", MySqlDbType.Binary) { Value = picture });
                    command.ExecuteNonQuery();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Ruft das Profilbild des Users ab.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public byte[] GetProfilePicture(string username)
        {
            if (Exists(_username))
            {
                using (MySqlCommand command = MySqlConnection.CreateCommand())
                {
                    command.CommandText = $"SELECT picture FROM user WHERE name='{_username}'";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        var image = new byte[65556];
                        if (reader.Read())
                            reader.GetBytes(0, 0, image, 0, image.Length);
                        return image;
                    }
                }
            }
            return null;
        }

        /// <summary>
        ///     Löscht den Benutzer
        /// </summary>
        /// <param name="username">Username des zu löschenden Users.</param>
        /// <param name="password">Passwort des zu löschenden Users.</param>
        /// <returns>True, wenn das Löschen erfolgreich war.</returns>
        public bool DeleteUser(string username, string password)
        {
            if (CheckLogin(username, password))
            {
                using (MySqlCommand command = MySqlConnection.CreateCommand())
                {
                    command.CommandText = $"DELETE FROM user WHERE name = '{username}' AND password = '{password}'";
                    command.ExecuteNonQuery();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gibt eine Liste der Spiele des users zurück
        /// </summary>
        /// <returns>Liste von igdb_urls.</returns>
        public List<string> GetGames()
        {
            //TODO: Testen von GetGames
            List<string> list = new List<string>();
            if (_isLoggedIn)
            {
                using (MySqlCommand command = MySqlConnection.CreateCommand())
                {
                    command.CommandText = $"SELECT igdb_url FROM gameinfo WHERE user_ID ='{_username}'";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read()) list.Add(reader.GetString(0));
                    }
                }
            }
            return list;
        }

        public DateTime GetPlayTime(string igdbUrl)
        {
            //TODO: Testen von GetPlayTime
            if (_isLoggedIn)
            {
                using (MySqlCommand command = MySqlConnection.CreateCommand())
                {
                    command.CommandText = $"SELECT playtime FROM gameinfo WHERE user_ID ='{_username}' AND igdb_url='{igdbUrl}'";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read()) return reader.GetDateTime(0);
                    }
                }
            }
            return default(DateTime);
        }

        public void AddPlayTime(string igdbUrl, TimeSpan playedTime)
        {
            //TODO: Testen von AddPlayTime
            DateTime newPlayTime = GetPlayTime(igdbUrl) + playedTime;
            if (_isLoggedIn)
            {
                using (MySqlCommand command = MySqlConnection.CreateCommand())
                {
                    command.CommandText = $"UPDATE gameinfo SET playtime='{newPlayTime.ToString("yyyy-MM-dd HH:mm:ss")}' WHERE user_ID='{_username}' AND igdb_url='{igdbUrl}'";
                }
            }
        }
    }
}