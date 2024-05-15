using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace UNIS_Inspired_Enrollment_System.Classes
{
    internal class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }

        public User() { }

        public User(int id, string username, string passwordHash, string passwordSalt)
        {
            Id = id;
            Username = username;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }

        public bool Login(string username, string password)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\DAN\\source\\repos\\UNIS-Inspired Enrollment System\\UNIS-Inspired Enrollment System\\Database.mdf;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT PasswordHash FROM Users WHERE Username = @Username", connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string hash = reader.GetString(0);

                            if (BCrypt.Net.BCrypt.Verify(password, hash))
                            {
                                Properties.Settings.Default.IsLoggedIn = true;
                                Properties.Settings.Default.LoggedInUser = username;
                                Properties.Settings.Default.Save();

                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        public bool Logout()
        {
            Properties.Settings.Default.IsLoggedIn = false;
            Properties.Settings.Default.LoggedInUser = null;
            Properties.Settings.Default.Save();

            return true;
        }

        public bool AddUser(string username, string password)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\DAN\\source\\repos\\UNIS-Inspired Enrollment System\\UNIS-Inspired Enrollment System\\Database.mdf;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand checkCommand = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Username = @Username", connection))
                {
                    checkCommand.Parameters.AddWithValue("@Username", username);
                    if ((int)checkCommand.ExecuteScalar() > 0)
                    {
                        return false;
                    }
                }

                string salt = BCrypt.Net.BCrypt.GenerateSalt();
                string hash = BCrypt.Net.BCrypt.HashPassword(password, salt);

                using (SqlCommand insertCommand = new SqlCommand("INSERT INTO Users (Username, PasswordHash, PasswordSalt) VALUES (@Username, @PasswordHash, @PasswordSalt)", connection))
                {
                    insertCommand.Parameters.AddWithValue("@Username", username);
                    insertCommand.Parameters.AddWithValue("@PasswordHash", hash);
                    insertCommand.Parameters.AddWithValue("@PasswordSalt", salt);

                    if (insertCommand.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool UpdateUser(int id, string username, string password)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\DAN\\source\\repos\\UNIS-Inspired Enrollment System\\UNIS-Inspired Enrollment System\\Database.mdf;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand checkCommand = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Username = @Username AND Id != @Id", connection))
                {
                    checkCommand.Parameters.AddWithValue("@Username", username);
                    checkCommand.Parameters.AddWithValue("@Id", id);
                    if ((int)checkCommand.ExecuteScalar() > 0)
                    {
                        return false;
                    }
                }

                string salt = BCrypt.Net.BCrypt.GenerateSalt();
                string hash = BCrypt.Net.BCrypt.HashPassword(password, salt);

                using (SqlCommand updateCommand = new SqlCommand("UPDATE Users SET Username = @Username, PasswordHash = @PasswordHash, PasswordSalt = @PasswordSalt WHERE Id = @Id", connection))
                {
                    updateCommand.Parameters.AddWithValue("@Username", username);
                    updateCommand.Parameters.AddWithValue("@PasswordHash", hash);
                    updateCommand.Parameters.AddWithValue("@PasswordSalt", salt);
                    updateCommand.Parameters.AddWithValue("@Id", id);

                    if (updateCommand.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public List<User> GetUsers()
        {
            List<User> users = new List<User>();

            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\DAN\\source\\repos\\UNIS-Inspired Enrollment System\\UNIS-Inspired Enrollment System\\Database.mdf;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT Id, Username, PasswordHash, PasswordSalt FROM Users", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User user = new User
                            {
                                Id = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                PasswordHash = reader.GetString(2),
                                PasswordSalt = reader.GetString(3)
                            };

                            users.Add(user);
                        }
                    }
                }
            }

            return users;
        }

        public bool DeleteUser(int id)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\DAN\\source\\repos\\UNIS-Inspired Enrollment System\\UNIS-Inspired Enrollment System\\Database.mdf;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand deleteCommand = new SqlCommand("DELETE FROM Users WHERE Id = @Id AND Username != 'admin'", connection))
                {
                    deleteCommand.Parameters.AddWithValue("@Id", id);
                    if (deleteCommand.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool AddAdminUser()
        {
            string adminUsername = "admin";
            string adminPassword = "admin";
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\DAN\\source\\repos\\UNIS-Inspired Enrollment System\\UNIS-Inspired Enrollment System\\Database.mdf;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand checkCommand = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Username = @Username", connection))
                {
                    checkCommand.Parameters.AddWithValue("@Username", adminUsername);
                    if ((int)checkCommand.ExecuteScalar() > 0)
                    {
                        return false;
                    }
                }

                string salt = BCrypt.Net.BCrypt.GenerateSalt();
                string hash = BCrypt.Net.BCrypt.HashPassword(adminPassword, salt);

                using (SqlCommand insertCommand = new SqlCommand("INSERT INTO Users (Username, PasswordHash, PasswordSalt) VALUES (@Username, @PasswordHash, @PasswordSalt)", connection))
                {
                    insertCommand.Parameters.AddWithValue("@Username", adminUsername);
                    insertCommand.Parameters.AddWithValue("@PasswordHash", hash);
                    insertCommand.Parameters.AddWithValue("@PasswordSalt", salt);

                    if (insertCommand.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
