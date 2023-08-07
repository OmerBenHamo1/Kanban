using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BuisnessLayer;
using System.Text.Json;
using System.Text.Json.Serialization;
using log4net;
using log4net.Config;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class BoardService : IBoardService
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);//This goes at each class
        public IUserFacade uf { get; set ; }
        public IBoardFacade bf { get; set; }

        public BoardService(IBoardFacade _bf, IUserFacade _uf)
        {
            this.uf = _uf;
            this.bf = _bf;
        }
        ///<summary>
        ///This method loads all persisted data.
        ///<\summary>
        /// <returns>JSON object with null, unless an error occurs </returns>
        public string LoadData()
        {
            try
            {
                bf.LoadData();
                Response response = new(null, null);
                Log.Info($"The data was loaded.");
                return JsonSerializer.Serialize(response);
            }
            catch (Exception e)
            {
                Response response = new(null, e.Message);
                string json = JsonSerializer.Serialize<Response>(response);
                Log.Error($"Load data failed");
                return json;
            }
        }
        ///<summary>This method deletes all persisted data.
        ///<para>
        ///<b>IMPORTANT:</b> 
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.
        ///</para>
        /// </summary>
        ///<returns>An empty response, unless an error occurs</returns>
        public string DeleteData()
        {
            try
            {
                bf.DeleteData();
                Response response = new(null, null);
                Log.Info($"The data was deleted.");
                return JsonSerializer.Serialize(response);
            }
            catch (Exception e)
            {
                Response response = new(null, e.Message);
                string json = JsonSerializer.Serialize<Response>(response);
                Log.Error($"delete data failed");
                return json;
            }
        }

        ///<summery>
        ///Creating a board for a user
        ///</summery>
        ///<param name= "email"> The user's email address, it is used as a username login to the system.</param>
        ///<param name = "boardName" > The name of the new board.</param>
        ///<returns>JSON object represents the new Board, or an error if accured.</returns>
        public string CreateBoard(string email, string boardName)
        {
            try
            {
                Board success = bf.CreateBoard(email, boardName);
                BoardToSend boardToSend = new(success);
                Response response = new(boardToSend, null);
                Log.Info($"A Board named {boardToSend.BoardName} was created for user: {email}.");
                return JsonSerializer.Serialize(response);
            }
            catch(Exception e)
            {
                Response response = new(null,e.Message);
                string json = JsonSerializer.Serialize<Response>(response);
                Log.Error($"Tried to create a Board. user {email} , boardName: {boardName}. Error: {e.Message}");
                return json;
            }
        }
        ///<summery>
        ///Deleting a board for a user
        ///</summery>
        ///<param name= "email"> The user's email address, it is used as a username login to the system.</param>
        ///<param name = "boardName" > The name of the board.</param>
        ///<returns>JSON object represents a text about the deleted board, or an error if accured.</returns>
        public string DeleteBoard(string email, string boardName)
        {
            try
            {
                string success = bf.DeleteBoard(email, boardName);
                Response response = new(success, null);
                Log.Info($"A Board named {boardName} was deleted for user: {email}.");
                return JsonSerializer.Serialize(response);
            }
            catch (Exception e)
            {
                Log.Error($"Tried to delete a Board. user {email}, boardName: {boardName}. Error: {e.Message}");
                return JsonSerializer.Serialize(new Response(null, e.Message));
            }   
        }
        ///<summery>
        ///Get a board for a user
        ///</summery>
        ///<param name= "email"> The user's email address, it is used as a username login to the system.</param>
        ///<param name = "boardName" > The name of the board.</param>
        ///<returns>JSON object represents the board, or an error if accured.</returns>
        public string GetBoard(string email, string boardName)
        {
            try
            {
                Board board = bf.GetBoard(email, boardName);
                BoardToSend boardToSend = new BoardToSend(board);
                Response response = new(boardToSend, null);
                Log.Info($"A Board named {boardToSend.BoardName} was returned for user: {email}.");
                return JsonSerializer.Serialize(response);   
            }
            catch (Exception e)
            {
                Log.Error($"Tried to get a Board. user {email}, boardName: {boardName}. Error: {e.Message}");
                return JsonSerializer.Serialize(new Response(null, e.Message));
            }
        }
        public string GetBoard(int boardID)
        {
            try
            {
                Board board = bf.GetBoard(boardID);
                BoardToSend boardToSend = new (board);
                Response response = new(boardToSend, null);
                Log.Info($"Board number {boardID} was returned");
                return JsonSerializer.Serialize(response);
            }
            catch (Exception e)
            {
                Log.Error($"Tried to get a Board number {boardID}. Error: {e.Message}");
                return JsonSerializer.Serialize(new Response(null, e.Message));
            }
        }
        ///<summery>
        ///Limiting the number of tasks in specific column for a user
        ///</summery>
        ///<param name= "email"> The user's email address, it is used as a username login to the system.</param>
        ///<param name = "boardName" > The name of the new board.</param>
        ///<param name = "columnOrdinal" > The number of column.</param>
        ///<param name = "limit" > The new limit.</param>
        ///<returns>JSON object represents a text about the success of the func, or an error if accured.</returns>
        public string LimitColumn(string email, string boardName,int columnOrdinal, int limit)
        {
            try
            {
                int success = bf.LimitColumn(email, boardName, columnOrdinal, limit);
                Response response = new(success, null);
                Log.Info($"Column number {columnOrdinal} in Board {boardName} of user: {email} was limited to {limit}.");
                return JsonSerializer.Serialize(response);
            }
            catch (Exception e)
            {
                Log.Error($"Tried to limit a column. user {email}, boardName: {boardName}, columnOrdnal: {columnOrdinal}, new limit: {limit}. Error: {e.Message}");
                return JsonSerializer.Serialize(new Response(null, e.Message));
            }
        }
        ///<summery>
        ///Getting the limit of a column for a user
        ///</summery>
        ///<param name= "email"> The user's email address, it is used as a username login to the system.</param>
        ///<param name = "boardName" > The name of the board.</param>
        ///<param name = "columnOrdinal" > The column number.</param>
        ///<returns>JSON object represents the limit of the column, or an error if accured.</returns>
        public string GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            try
            {
                int success = bf.GetColumnLimit(email, boardName, columnOrdinal);
                Response response = new(success, null);
                Log.Info($"The column number {columnOrdinal}'s limit in Board {boardName} was returned to user: {email}.");
                return JsonSerializer.Serialize(response);
            }
            catch (Exception e)
            {
                Log.Error($"Tried to get limit of a column. user {email}, boardName: {boardName}, columnOrdnal: {columnOrdinal}. Error: {e.Message}");
                return JsonSerializer.Serialize(new Response(null, e.Message));
            }
        }
        ///<summery>
        ///Getting the name of a column for a user
        ///</summery>
        ///<param name= "email"> The user's email address, it is used as a username login to the system.</param>
        ///<param name = "boardName" > The name of the board.</param>
        ///<param name = "columnOrdinal" > The column number.</param>
        ///<returns>JSON object represents the name of the column, or an error if accured.</returns>
        public string GetColumnName(string email, string boardName, int columnOrdinal)
        {
            try
            {
                string success = bf.GetColumnName(email, boardName, columnOrdinal);
                Response response = new(success, null);
                Log.Info($"The column name, with columnOrdinal: {columnOrdinal} in Board {boardName} was returned to user: {email}.");
                return JsonSerializer.Serialize(response);
            }
            catch (Exception e)
            {
                Log.Error($"Tried to get name  of a column. user {email}, boardName: {boardName}, columnOrdnal: {columnOrdinal}. Error: {e.Message}");
                return JsonSerializer.Serialize(new Response(null, e.Message));
            }
        }
        ///<summery>
        ///Adding a new task to backlog column
        ///</summery>
        ///<param name= "email"> The user's email address, it is used as a username login to the system.</param>
        ///<param name = "boardName" > The name of the board to get the task from.</param>
        ///<param name = "columnOrdinal" > The column ordinal .</param>
        ///<returns>JSON object represents the column that we want to get, or an error if accured.</returns>
        public string GetColumn(string email, string boardName, int columnOrdinal)
        {
            try
            {
                List<ITask> column = bf.GetColumn(email, boardName, columnOrdinal);
                List<TaskToSend> tasksToSend = new List<TaskToSend>();
                foreach (ITask task in column)
                {
                    tasksToSend.Add(new TaskToSend(task));
                }
                Response response = new(tasksToSend, null);
                Log.Info($"The column was returned for user: {email}.");
                return JsonSerializer.Serialize(response);
            }
            catch (Exception e)
            {
                Response response = new(null, e.Message);
                string json = JsonSerializer.Serialize<Response>(response);
                Log.Error($"Tried to updaet the title in a Task. user {email} , boardName: {boardName}. Error: {e.Message}");
                return json;
            }
        }
        /// <summary>
        /// This method returns a list of IDs of all user's boards.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <returns>A response with a list of IDs of all user's boards, unless an error occurs</returns>
        public string GetUserBoards(string email)
        {
            try
            {
                List<int> boards= bf.GetUserBoards(email);
                Response response = new(boards, null);
                Log.Info($"GetUserBoards was success for email: {email}");
                return JsonSerializer.Serialize(response);
            }
            catch (Exception e)
            {
                Response response = new(null, e.Message);
                string json = JsonSerializer.Serialize<Response>(response);
                Log.Error($"Tried to GetUserBoards, but failed. email: {email} . Error: {e.Message}");
                return json;
            }
        }
        /// <summary>
        /// This method adds a user as member to an existing board.
        /// </summary>
        /// <param name="email">The email of the user that joins the board. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>An empty response, unless an error occurs</returns>
        public string JoinBoard(string email, int boardID)
        {
            try
            {
                bf.JoinBoard(email,boardID);
                Response response = new(null, null);
                Log.Info($"JoinBoard was success for email: {email}, board id: {boardID}");
                return JsonSerializer.Serialize(response);
            }
            catch (Exception e)
            {
                Response response = new(null, e.Message);
                string json = JsonSerializer.Serialize<Response>(response);
                Log.Error($"Tried to JoinBoard, but failed. email: {email} . Error: {e.Message}");
                return json;
            }
        }
        /// <summary>
        /// This method removes a user from the members list of a board.
        /// </summary>
        /// <param name="email">The email of the user. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>An empty response, unless an error occurs</returns>
        public string LeaveBoard(string email, int boardID)
        {
            try
            {
                bf.LeaveBoard(email, boardID);
                Response response = new(null, null);
                Log.Info($"LeaveBoard was success for email: {email}, board id: {boardID}");
                return JsonSerializer.Serialize(response);
            }
            catch (Exception e)
            {
                Response response = new(null, e.Message);
                string json = JsonSerializer.Serialize<Response>(response);
                Log.Error($"Tried to LeaveBoard, but failed. email: {email} . Error: {e.Message}");
                return json;
            }
        }
        /// <summary>
        /// This method returns a board's name
        /// </summary>
        /// <param name="boardId">The board's ID</param>
        /// <returns>A response with the board's name, unless an error occurs</returns>
        public string GetBoardName(int boardId)
        {
            try
            {
                string board = bf.GetBoardName(boardId);
                Response response = new(board, null);
                Log.Info($"GetBoardName was success for board id: {boardId}");
                return JsonSerializer.Serialize(response);
            }
            catch (Exception e)
            {
                Response response = new(null, e.Message);
                string json = JsonSerializer.Serialize<Response>(response);
                Log.Error($"Tried to GetBoardName, but failed. board id: {boardId} . Error: {e.Message}");
                return json;
            }
        }
    }
}