using IntroSE.Kanban.Backend.BuisnessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class TaskController
    {
        public UserController Us { get; set; }
        private readonly string connectionString;
        public TaskController(string dbPath, UserController _uc)
        {
            SQLiteConnectionStringBuilder builder = new() { DataSource = dbPath };
            connectionString = $"Data Source={dbPath};Version=3;";
            Us = _uc;
        }
        public List<List<DTOTask>> GetAllTasksOfBoard(int boardId)
        {
            List<List<DTOTask>> Columns = new List<List<DTOTask>>
            {
                new List<DTOTask>(),
                new List<DTOTask>(),
                new List<DTOTask>()
            };
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                string query2 = $"SELECT * FROM Tasks WHERE boardID = @boardId";
                using (SQLiteCommand command = new SQLiteCommand(query2, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@boardId", boardId);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int Id = reader.GetInt32(0);
                            string title = reader.GetString(1);
                            string description = reader.GetString(2);
                            string dueDate = reader.GetString(3);
                            string creationTime = reader.GetString(4);
                            int boardID = reader.GetInt32(5);
                            int column = reader.GetInt32(6);
                            int isPersisted= reader.GetInt32(7);
                            string assigne;
                            if (!reader.IsDBNull(8))
                                assigne = reader.GetString(8);
                            else
                                assigne = null;
                            DTOTask task = new (this, 1, Id, title, description, dueDate, creationTime, assigne, boardId, column);
                            Columns[column].Add(task);
                        }
                    }
                }
            }
            return Columns;
        }
        public void DeleteTask(DTOTask Dt)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string deleteQuery = "DELETE FROM Tasks WHERE taskID = @taskId";
                using (SQLiteCommand command = new SQLiteCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@taskId", Dt.TaskId);
                    int rowsAffected = command.ExecuteNonQuery();
                }
            }
        }
        public void DeleteAllTasks(int boardID)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string deleteQuery = "DELETE FROM Tasks WHERE boardID = @boardId";
                using (SQLiteCommand command = new SQLiteCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@boardId", boardID);
                    int rowsAffected = command.ExecuteNonQuery();
                }
            }
        }
        public void DeleteAllTasks()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Delete all data from the Users table
                string query = "DELETE FROM Tasks";
                using (SQLiteCommand deleteUserCommand = new SQLiteCommand(query, connection))
                {
                    deleteUserCommand.ExecuteNonQuery();
                }
                // Commit the changes and close the connection
                connection.Close();
            }
        }
        public void UnAssignTasks(string email, int boardID)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string updateQuery = "UPDATE Tasks SET assigne = NULL WHERE assigne = @email AND boardID = @boardID AND Column < 2";
                using (SQLiteCommand command = new SQLiteCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@boardID", boardID);
                    int rowsAffected = command.ExecuteNonQuery();
                }
            }
        }
        public void AdvanceTask(int taskId,int coloumnOrdinal)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Tasks SET column = @columnOrdinal WHERE taskID = @taskId";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@columnOrdinal", coloumnOrdinal);
                    command.Parameters.AddWithValue("@taskId", taskId);//Not necessary
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected <= 0)
                        throw new Exception("Update task title failed.");
                }
            }
            
        }


        public void UpdateTaskTitle(int taskId, string newTitle)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Tasks SET title = @newTitle WHERE taskID = @taskId";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@newTitle", newTitle);
                    command.Parameters.AddWithValue("@taskId", taskId);//Not necessary
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected <= 0)
                        throw new Exception("Update task title failed.");
                }
            }
        }
        public void UpdateTaskDescription(int taskId, string newDescription)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Tasks SET description = @newDescription WHERE taskID = @taskId";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@newDescription", newDescription);
                    command.Parameters.AddWithValue("@taskId", taskId);//Not necessary
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected <= 0)
                        throw new Exception("Update task description failed.");
                }
            }
        }
        public void UpdateTaskDueDate(int taskId, string newDueDate)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Tasks SET dueDate = @newDueDate WHERE taskID = @taskId";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@newDueDate", newDueDate);
                    command.Parameters.AddWithValue("@taskId", taskId);//Not necessary
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected <= 0)
                        throw new Exception("Update duedate failed.");
                }
            }
            
        }
        public void AddTask(DTOTask task)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Tasks (taskID, title, description, dueDate, creationTime, boardID, column, isPersisted,assigne) VALUES (@taskID, @title, @description, @dueDate, @creationTime, @boardID, @column, @isPersisted, @email)";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@taskID", task.TaskId);
                    command.Parameters.AddWithValue("@title", task.Title);
                    command.Parameters.AddWithValue("@description", task.Description);
                    command.Parameters.AddWithValue("@dueDate", task.DueDate);
                    command.Parameters.AddWithValue("@creationTime", task.CreationTime);
                    command.Parameters.AddWithValue("@boardID", task.BoardID);
                    command.Parameters.AddWithValue("@column", 0);
                    command.Parameters.AddWithValue("isPersisted", 1);
                    command.Parameters.AddWithValue("@email", task.Assignee);

                    command.ExecuteNonQuery();
                }
            }
        }
        public void AssignTask(int taskId, int boardId, string emailAssignee)
        {

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Tasks SET assigne = @emailAssignee WHERE taskID = @taskId";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@emailAssignee", emailAssignee);
                    command.Parameters.AddWithValue("@taskId", taskId);//Not necessary
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected <= 0)
                            throw new Exception("Task not found or assign failed.");
                }
            }  
        }
    }
}



