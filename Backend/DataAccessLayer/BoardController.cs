using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using IntroSE.Kanban.Backend.BuisnessLayer;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class BoardController
    {
        public UserController Uc;
        public TaskController Tc;
        private readonly string connectionString;

        public BoardController(string dbPath, UserController _uc, TaskController _tc)
        {
            SQLiteConnectionStringBuilder builder = new() { DataSource = dbPath };
            connectionString = $"Data Source={dbPath};Version=3;";
            Uc = _uc;
            Tc = _tc;
        }        
        public void Insert(DTOBoard board)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO Boards (boardID, boardName,email,limitTaskInColumn0,limitTaskInColumn1,limitTaskInColumn2,isPersisted) VALUES ( @boardID, @boardName, @email, @limitTaskInColumn0, @limitTaskInColumn1, @limitTaskInColumn2, @isPersisted)";
                using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@boardID", board.Id);
                    command.Parameters.AddWithValue("@boardName", board.boardName);
                    command.Parameters.AddWithValue("@email", board.email);
                    command.Parameters.AddWithValue("@limitTaskInColumn0", board.limitTaskInColumn0);
                    command.Parameters.AddWithValue("@limitTaskInColumn1", board.limitTaskInColumn1);
                    command.Parameters.AddWithValue("@limitTaskInColumn2", board.limitTaskInColumn2);
                    command.Parameters.AddWithValue("@isPersisted", 1);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        board.isPersisted = 1;
                    }
                    else
                    {
                        throw new Exception("Board insertion failed.");
                    }
                }
                AddToMembers(board.email, board.Id);
            }
        }
        public void AddToMembers(string email, int boardID)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO Members (email, boardID) VALUES (@email, @boardID)";
                using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@boardID", boardID);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected <= 0)
                        throw new Exception("Member insertion failed.");
                }
            }
        }
        public List<DTOBoard> GetAllBoards()
        {
            List<DTOBoard> boards = new List<DTOBoard>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Boards";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int boardId = reader.GetInt32(0);
                            string boardName = reader.GetString(1);
                            string email = reader.GetString(2);
                            int limitTaskInColumn0 = reader.GetInt32(3);
                            int limitTaskInColumn1 = reader.GetInt32(4);
                            int limitTaskInColumn2 = reader.GetInt32(5);

                            DTOBoard board = new DTOBoard(this, 1, boardId, boardName, email, limitTaskInColumn0, limitTaskInColumn1, limitTaskInColumn2);
                            board.Columns = this.Tc.GetAllTasksOfBoard(boardId);

                            List<string> members = new List<string>();
                            string query2 = "SELECT email FROM Members WHERE BoardId = @boardId";
                            using (SQLiteCommand memberCommand = new SQLiteCommand(query2, connection))
                            {
                                memberCommand.Parameters.AddWithValue("@boardId", boardId);
                                using (SQLiteDataReader memberReader = memberCommand.ExecuteReader())
                                {
                                    while (memberReader.Read())
                                    {
                                        string memberEmail = memberReader.GetString(0);
                                        members.Add(memberEmail);
                                    }
                                }
                            }
                            board.Members = members;

                            boards.Add(board);
                        }
                    }
                }
            }
            return boards;
        }

        public void DeleteBoard(DTOBoard board)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string deleteQuery = "DELETE FROM Boards WHERE boardID = @boardID";
                using (SQLiteCommand command = new SQLiteCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@boardID", board.Id);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected <= 0)
                        throw new Exception("Board not found or deletion failed.");
                }
            }
            RemoveMember(board.email, board.Id);
            Tc.DeleteAllTasks(board.Id);
        }
        public void DeleteTask (DTOTask Dt)
        {
            Tc.DeleteTask(Dt);
        }
        public void RemoveMember(string email, int boardID)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string deleteQuery = "DELETE FROM Members WHERE boardID = @boardID AND email = @email";
                using (SQLiteCommand command = new SQLiteCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@boardID", boardID);
                    command.Parameters.AddWithValue("@email", email);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected <= 0)
                        throw new Exception("Member not found or removale failed.");
                }
            }
        }
        public void DeleteAllBoards()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Delete all data from the Users table
                string query = "DELETE FROM Boards";
                using (SQLiteCommand deleteUserCommand = new SQLiteCommand(query, connection))
                {
                    deleteUserCommand.ExecuteNonQuery();
                }
                // Commit the changes and close the connection
                connection.Close();
            }
        }
        public void DeleteAllMembers()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Delete all data from the Users table
                string query = "DELETE FROM Members";
                using (SQLiteCommand deleteUserCommand = new SQLiteCommand(query, connection))
                {
                    deleteUserCommand.ExecuteNonQuery();
                }
                // Commit the changes and close the connection
                connection.Close();
            }
        }
        public void DeleteAllTasks()
        {
            Tc.DeleteAllTasks();
        }
        public void TransferOwnership(int boardID, string email)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string updateQuery = "UPDATE Boards SET email = @email WHERE boardID = @boardID";
                using (SQLiteCommand command = new SQLiteCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@boardID", boardID);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected <= 0) throw new Exception("update owner failed");
                }
            }
        }
        public void UnAssignTasks(string email, int boardID)
        {
            Tc.UnAssignTasks(email, boardID);
        }

        public void AddTask(DTOTask task)
        {
            Tc.AddTask(task);
        }

        public void AssignTask(int taskId, int boardId, string emailAssignee)
        {
            Tc.AssignTask(taskId, boardId, emailAssignee);
        }

        //public DTOBoard LimitColumn(int boardId, int columnOrdinal, int limit)
        //{
        //    DTOBoard board = null;
        //    using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        //    {
        //        // connection.Open();
        //        string query = $"UPDATE Boards SET limitTaskInColumn0 = @limit WHERE columnOrdinal = @columnOrdinal";

        //        // if (columnOrdinal == 0) { query = $"UPDATE Boards SET limitTaskInColumn0 = @limit WHERE columnOrdinal = @columnOrdinal"; }
        //        // else if (columnOrdinal == 1) { query = $"UPDATE Boards SET limitTaskInColumn1 = @limit WHERE columnOrdinal = @columnOrdinal"; }
        //        // else { query = $"UPDATE Boards SET limitTaskInColumn2 = @limit WHERE columnOrdinal = @columnOrdinal"; }
        //        using (SQLiteCommand command = new SQLiteCommand(query, connection))
        //        {
        //            connection.Open();
        //            command.Parameters.AddWithValue("@limit", limit);
        //            command.Parameters.AddWithValue("@boardId", boardId);

        //            string query2 = "SELECT * FROM Boards WHERE boardId = @boardId";
        //            using (SQLiteCommand selectCommand = new SQLiteCommand(query2, connection))
        //            {
        //                selectCommand.Parameters.AddWithValue("@boardId", boardId);
        //                {
        //                    using (SQLiteDataReader reader = selectCommand.ExecuteReader())
        //                    {
        //                        if (reader.Read())
        //                        {
        //                            string boardName = reader.GetString(1);
        //                            string email = reader.GetString(2);
        //                            int limitTaskInColumn0 = reader.GetInt32(3);
        //                            int limitTaskInColumn1 = reader.GetInt32(4);
        //                            int limitTaskInColumn2 = reader.GetInt32(5);
        //                            board = new DTOBoard(this, 1, boardId, boardName, email, limitTaskInColumn0, limitTaskInColumn1, limitTaskInColumn2);
        //                        }
        //                    }
        //                }


        //            }
        //            return board;
        //        }
        //    }
        //}
        public DTOBoard LimitColumn(int boardId, int columnOrdinal, int limit)
        {
            DTOBoard board = null;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string columnNum = $"limitTaskInColumn{columnOrdinal}";
                string query = $"UPDATE Boards SET {columnNum} = @limit WHERE boardId = @boardId";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@limit", limit);
                    command.Parameters.AddWithValue("@boardId", boardId);

                    command.ExecuteNonQuery();
                }

                string selectQuery = "SELECT * FROM Boards WHERE boardId = @boardId";
                using (SQLiteCommand selectCommand = new SQLiteCommand(selectQuery, connection))
                {
                    selectCommand.Parameters.AddWithValue("@boardId", boardId);

                    using (SQLiteDataReader reader = selectCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string boardName = reader.GetString(1);
                            string email = reader.GetString(2);
                            int limitTaskInColumn0 = reader.GetInt32(3);
                            int limitTaskInColumn1 = reader.GetInt32(4);
                            int limitTaskInColumn2 = reader.GetInt32(5);
                            board = new DTOBoard(this, 1, boardId, boardName, email, limitTaskInColumn0, limitTaskInColumn1, limitTaskInColumn2);
                        }
                    }
                }

                connection.Close();
            }

            return board;
        }

        public void AdvanceTask(int taskId, int newColoumnOrdinal)
        {
             Tc.AdvanceTask(taskId,newColoumnOrdinal);
        }

        public void UpdateTaskTitle(int taskId, string newTitle)
        {
             Tc.UpdateTaskTitle(taskId, newTitle);
        }

        public void UpdateTaskDescription(int taskId, string newDescription)
        {
             Tc.UpdateTaskDescription(taskId, newDescription);
        }

        public void UpdateTaskDueDate(int taskId, string newDueDate)
        {
             Tc.UpdateTaskDueDate(taskId, newDueDate);
        }

    }
}
