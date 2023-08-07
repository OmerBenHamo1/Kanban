using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class UserController
    {
        private readonly string connectionString;

        public UserController(string dbPath)
        {
            SQLiteConnectionStringBuilder builder = new() { DataSource = dbPath };
            connectionString = $"Data Source={dbPath};Version=3;";
        }
        public List<DTOUser> GetAllUsers()
        {// here you need to make a query to get all users from the data base.
            List<DTOUser> users = new List<DTOUser>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Users"; 
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string email = reader.GetString(0);
                            string password = reader.GetString(1);
                            int isloggedin = reader.GetInt32(2);
                            int isPersisted = reader.GetInt32(3);

                            // Create a DTOUser object and add it to the list
                            DTOUser user = new DTOUser(this, email, password, isloggedin, isPersisted);
                            users.Add(user);
                        }
                    }
                }
            }
            return users;
        }
        public DTOUser SelectUser(string email)
        {
            DTOUser user = null;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = $"SELECT * FROM Users WHERE email = @Email";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string uemail = reader.GetString(0);
                            string password = reader.GetString(1);
                            int loggedIn = reader.GetInt32(2);
                            int isPersisted = reader.GetInt32(3);
                            user = new DTOUser(this, uemail, password,loggedIn, isPersisted);
                        }
                    }
                }
            }
            return user;
        }

        public void Register(string email, string password)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Users (email, password, loggedIn, isPersisted) VALUES (@Email, @Password, @loggedIn, @isPersisted); SELECT last_insert_rowid();";
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = query;
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@loggedIn", 1);
                    command.Parameters.AddWithValue("@isPersisted", 1);
                    command.ExecuteNonQuery();
                }
            }
        }

        public DTOUser ChangePassword(string email, string newPassword)
        {
            DTOUser updatedUser = null;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Users SET password = @Password WHERE Email = @Email";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Password", newPassword);
                    command.Parameters.AddWithValue("@Email", email);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        updatedUser = new DTOUser(this, email, newPassword,1,1);
                    }
                }
            }
            return updatedUser;
        }

        public DTOUser ChangeEmail(string currentEmail, string newEmail, string password)
        {
            DTOUser updatedUser = null;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Users SET email = @NewEmail WHERE email = @CurrentEmail";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NewEmail", newEmail);
                    command.Parameters.AddWithValue("@CurrentEmail", currentEmail);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        updatedUser = new DTOUser(this, newEmail, password,1,1);
                    }
                }
            }
            return updatedUser;
        }

        public DTOUser Login(string email, string password)
        {
            DTOUser loggedInUser = null;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Users SET loggedIn = 1 WHERE email = @Email";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string uemail = reader.GetString(1);
                            string upassword = reader.GetString(2);
                            loggedInUser = new DTOUser(this, uemail, upassword,1,1);
                        }
                    }
                }
            }
            return loggedInUser;
        }

        public int Logout(string email)
        {
            int logoutSuccessful = 0;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Users SET loggedIn = 0 WHERE email = @Email";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    logoutSuccessful = 1;
                }
            }
            return logoutSuccessful;
        }

        public void DeleteAllData()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Delete all data from the Users table
                string query = "DELETE FROM Users";
                using (SQLiteCommand deleteUserCommand = new SQLiteCommand(query, connection))
                {
                    deleteUserCommand.ExecuteNonQuery();
                }


                // Commit the changes and close the connection
                connection.Close();
            }
        }
    }
}
