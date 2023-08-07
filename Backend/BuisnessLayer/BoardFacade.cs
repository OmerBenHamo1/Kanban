using IntroSE.Kanban.Backend.DataAccessLayer;

using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    public class BoardFacade : IBoardFacade
    {
        public Dictionary<string,List<Board>> Boards { get; set; }
        public IUserFacade Uf { get; set; }
        public IUserHistory Uh { get;}
        public int numberOfBoardTotal { get; set; }
        public int numberOfTaskTotal { get; set;}
        public ExceptionThrower Et { get; set; }
        private BoardController Bc { get; }

        public BoardFacade (IUserFacade _uf, IUserHistory _uh, BoardController _Bc)
        {
            this.Boards = new Dictionary<string,List<Board>> ();
            this.Uf = _uf;
            this.Uh = _uh;
            this.numberOfTaskTotal = 0;
            this.numberOfBoardTotal = 0;
            this.Et = new();
            this.Bc = _Bc;
        }
        /// <summary>
        /// Loads the data from the data base.
        /// </summary>
        public void LoadData()
        {
            Uf.LoadData();
            List<DTOBoard> dtb = Bc.GetAllBoards();
            numberOfBoardTotal = dtb.Count;
            foreach(DTOBoard b in dtb)
            {
                Board board = new Board(b);
                for (int i = 0; i < board.Columns.Count; i++)
                {
                    numberOfTaskTotal += board.Columns[i].Count;
                }
                if (Boards.ContainsKey(board.email))
                {
                    List<Board> existingBoards = Boards[board.email];
                    existingBoards.Add(board);
                    Boards[board.email] = existingBoards;
                }
                else
                {
                    List<Board> newBoardList = new List<Board> { board };
                    Boards.Add(board.email, newBoardList);
                }
                Uh.AddBoardToOwner(board.email, board);
                foreach (var t in board.Columns[1])
                {
                    Uh.AddTaskInProgress(board.email, t);
                }
            }
        }
        /// <summary>
        /// Deletes the data from the data base and from the RAM.
        /// </summary>
        public void DeleteData()
        {
            IsAdded();
            Uf.DeleteData();
            Uh.DeleteData();
            this.numberOfBoardTotal = 0;
            this.numberOfTaskTotal = 0;
            this.Boards.Clear();
            Bc.DeleteAllBoards();
            Bc.DeleteAllTasks();
            Bc.DeleteAllMembers();
        }
        /// <summary>
        /// Return the specific Board of a User
        /// </summary>
        /// <param name="email">represents the user's email</param>
        /// <param name="boardName">represents the board's name</param>
        /// <returns>The Board</returns>
        public Board GetBoard(string email, string boardName)
        {
            IsAdded();
            Et.EmailNoExist(Uf, email);
            User user = Uf.GetUser(email);
            Et.UserLoggedIn(user);
            Board board = null;
            foreach (var b in Boards[email])
            {
                if (b.boardName == boardName)
                {
                    board = b;
                    break;
                }
            }
            Et.BoardNotFound(board);
            return board;
        }
        /// <summary>
        /// Deletes the specific Board of a User
        /// </summary>
        /// <param name="email">represents the user's email</param>
        /// <param name="boardName">represents the board's name</param>
        /// <returns>A massage with the name f the deleted board</returns>
        public string DeleteBoard(string email, string boardName)
        {
            IsAdded();
            Board board = GetBoard(email, boardName);
            Et.IsBoardOwner(board, email);
            foreach (var t in board.Columns[1])
            {
                Uh.DeleteTask(t.Assignee, t.Id);
            }
            Uh.BoardsOfUsers[email].Remove(board.Id);
            Boards[email].Remove(board);
            Bc.DeleteBoard(board.DTOBoard);
            return "Board " + boardName +" was deleted successfully!";
        }
        /// <summary>
        /// Creates a Board for a User
        /// </summary>
        /// <param name="email">represents the user's email</param>
        /// <param name="boardName">represents the board's name</param>
        /// <returns>The Board</returns>
        public Board CreateBoard(string email, string boardName)
        {
            IsAdded();
            Et.IsNullOrWhiteSpace(boardName);
            Et.EmailNoExist(Uf, email);
            User user = Uf.GetUser(email);
            Et.UserLoggedIn(user);
            if (!Boards.ContainsKey(email))
                Boards[email] = new List<Board>();
            else 
                Et.IsBoardExist(Boards, email, boardName);
            Board board = new (this.numberOfBoardTotal, email, boardName, this.Bc);
            this.numberOfBoardTotal += 1;
            Boards[email].Add(board);
            board.Members.Add(email);
            Uh.AddBoardToOwner(email,board);
            Bc.Insert(board.DTOBoard);
            return board;
        }
        /// <summary>
        /// Returns the User's Boards
        /// </summary>
        /// <param name="email">represents the user's email</param>
        /// <returns>The User's Boards</returns>
        public List<int> GetUserBoards(string email)
        {
            IsAdded();
            Et.EmailNoExist(Uf, email);
            User user = Uf.GetUser(email);
            Et.UserLoggedIn(user);
            return Uh.GetBoardsOfUser(email);
        }
        /// <summary>
        /// This method adds a user as member to an existing board.
        /// </summary>
        /// <param name="email">The email of the user that joins the board. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>void, unless an error occurs</returns>
        public void JoinBoard(string email, int boardID)
        {
            IsAdded();
            Et.LegalId(boardID, numberOfBoardTotal);
            Et.EmailNoExist(Uf, email);
            User user = Uf.GetUser(email);
            Et.UserLoggedIn(user);
            Board board = GetBoard(boardID);
            Et.IsAMember(board, email);
            board.Members.Add(email);
            Uh.AddBoardToOwner(email, board);
            Bc.AddToMembers(email, boardID);
        }
        public Board GetBoard(int boardID)
        {
            IsAdded();
            Board board = null;
            foreach (var e in  Boards.Keys)
            {
                foreach (var b in Boards[e])
                {
                    if (b.Id == boardID)
                    {
                        board = b;
                        break;
                    }
                }
                if (board != null)
                    break;
            }
            Et.BoardNotFound(board);
            return board;
        }

        /// <summary>
        /// This method removes a user from the members list of a board.
        /// </summary>
        /// <param name="email">The email of the user. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>void, unless an error occurs</returns>
        public void LeaveBoard(string email, int boardID)
        {
            IsAdded();
            Et.LegalId(boardID,numberOfBoardTotal);
            Et.EmailNoExist(Uf, email);
            User user = Uf.GetUser(email);
            Et.UserLoggedIn(user);
            Board board = GetBoard(boardID);
            Et.IsNotBoardOwner(board, email);
            Et.IsNotMember(board,email);
            board.Members.Remove(email);
            board.UnAssign(email);
            //remove tasks in progress that he was assigned for
            foreach (var t in board.Columns[1])
            {
                if(t.Assignee == email)
                    Uh.DeleteTask(email, t.Id);
            }
            Uh.BoardsOfUsers[email].Remove(boardID);
            Bc.RemoveMember(email, boardID);
            Bc.UnAssignTasks(email, boardID);
        }
        /// <summary>
        /// This method returns a board's name
        /// </summary>
        /// <param name="boardId">The board's ID</param>
        /// <returns>The Board's name, unless an error occurs</returns>
        public string GetBoardName(int boardId)
        {
            IsAdded();
            Et.LegalId(boardId,numberOfBoardTotal);
            Board board = GetBoard(boardId);
            return board.boardName;
        }
        /// <summary>
        /// This method transfers a board ownership.
        /// </summary>
        /// <param name="currentOwnerEmail">Email of the current owner. Must be logged in</param>
        /// <param name="newOwnerEmail">Email of the new owner</param>
        /// <param name="boardName">The name of the board</param>
        /// <returns>void, unless an error occurs</returns>
        public void TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            IsAdded();
            Et.EmailNoExist(Uf, newOwnerEmail);
            Board board = GetBoard(currentOwnerEmail, boardName);
            Et.IsBoardOwner(board, currentOwnerEmail);
            Et.IsNotMember(board, newOwnerEmail);
            board.email = newOwnerEmail;
            Bc.TransferOwnership(board.Id, newOwnerEmail);
        }
        /// <summary>
        /// Adds a new Task for a User
        /// </summary>
        /// <param name="email">represents the user's email</param>
        /// <param name="boardName">represents the board's name</param>
        /// <param name="title">represents the task's title</param>
        /// <param name="description">represents the task's description</param>
        /// <param name="dueDate">represents the task's dueDate</param>
        /// <returns>The Task</returns>
        public ITask AddTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
            IsAdded();
            Et.IsNullOrWhiteSpace(title);
            Et.TitleLength(title, 50);
            Board board = GetBoard(email, boardName);
            Et.IsBoardOwner(board, email);
            Et.IsNotMember(board, email);
            ITask task = board.AddTask(this.numberOfTaskTotal, title, description, dueDate, Bc.Tc);
            Bc.AddTask(task.Dt);
            this.numberOfTaskTotal++;
            return task;
        }

        /// <summary>
        /// This method assigns a task to a user
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column number. The first column is 0, the number increases by 1 for each column</param>
        /// <param name="taskID">The task to be updated identified a task ID</param>        
        /// <param name="emailAssignee">Email of the asignee user</param>
        /// <returns>void, unless an error occurs </returns>
        public void AssignTask(string email, string boardName, int columnOrdinal, int taskId, string emailAssignee)
        {
            IsAdded();
            Et.LegalId(taskId, numberOfTaskTotal);
            Et.LegalColumnOrdinal(columnOrdinal);
            Board board = GetBoard(email, boardName);
            Et.IsNotMember(board, emailAssignee);
            ITask task = board.AssignTask(email, columnOrdinal, taskId, emailAssignee);
            if (columnOrdinal == 1)
            {
                Uh.DeleteTask(email, taskId);
                Uh.AddTaskInProgress(emailAssignee, task);
            }
            int boardId = board.DTOBoard.Id;
            Bc.AssignTask(taskId, boardId, emailAssignee);
        }

        ///<summery>
        ///Deletes a Task
        ///</summery>
        ///<param name= "email"> The user's email address, it is used as a username login to the system.</param>
        ///<param name = "boardName" > The name of the board to add the task.</param>
        ///<param name = "taskId" > The Task's id.</param>
        ///<returns>A text about the deleted Task</returns>
        public string DeleteTask(string email, string boardName, int taskId)
        {
            IsAdded();
            Et.LegalId(taskId, numberOfTaskTotal);
            Board board = GetBoard(email, boardName);
            ITask deletedTask =  board.DeleteTask(email,taskId);
            if(deletedTask.Column == 1)
                Uh.DeleteTask(email,taskId);
            Bc.DeleteTask(deletedTask.Dt);
            return "the task was deleted successfully";
        }
        /// <summary>
        /// Raturns a Task for a User
        /// </summary>
        /// <param name="email">represents the user's email</param>
        /// <param name="boardName">represents the board's name</param>
        /// <param name="id">represents the Task's id</param>
        /// <returns>The Task</returns>
        public ITask GetTask(string email, string boardName, int id)
        {
            IsAdded();
            Board board = GetBoard(email, boardName);
            return board.GetTask(id);
        }
        /// <summary>
        /// Limits the number of Tasks in column
        /// </summary>
        /// <param name="email">represents the user's email</param>
        /// <param name="boardName">represents the board's name</param>
        /// <param name="columnOrdinal">represents the column</param>
        /// <param name="limit">represents the new limit</param>
        /// <returns>The new limit</returns>
        public int LimitColumn(string email, string boardName, int columnOrdinal, int limit)
        {
            IsAdded();
            Et.LegalColumnOrdinal(columnOrdinal);
            Board board = GetBoard(email, boardName);
            Et.IsBoardOwner(board, email);
            Et.IsNotMember(board, email);
            int boardId = board.DTOBoard.Id;
            Bc.LimitColumn(boardId, columnOrdinal, limit);
            return board.LimitColumn(columnOrdinal, limit);
        }

        /// <summary>
        /// returns a column
        /// </summary>
        /// <param name="email">represents the user's email</param>
        /// <param name="boardName">represents the board's name</param>
        /// <param name="columnOrdinal">represents the column</param>
        /// <returns>The column</returns>
        public List<ITask> GetColumn(string email, string boardName, int columnOrdinal)
        {
            IsAdded();
            Et.LegalColumnOrdinal(columnOrdinal);
            Board board= GetBoard(email, boardName);
            return board.GetColumn(columnOrdinal);
        }
        /// <summary>
        /// Raturns the column's limit
        /// </summary>
        /// <param name="email">represents the user's email</param>
        /// <param name="boardName">represents the board's name</param>
        /// <param name="columnOrdinal">represents the column</param>
        /// <returns>The limit of the column</returns>
        public int GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            IsAdded();
            Et.LegalColumnOrdinal(columnOrdinal);
            Board board = GetBoard(email, boardName);
            return board.GetColumnLimit(columnOrdinal);

        }
        /// <summary>
        /// Raturns the column's name
        /// </summary>
        /// <param name="email">represents the user's email</param>
        /// <param name="boardName">represents the board's name</param>
        /// <param name="columnOrdinal">represents the column</param>
        /// <returns>The name of the column</returns>
        public string GetColumnName(string email, string boardName, int columnOrdinal)
        {
            IsAdded();
            Et.LegalColumnOrdinal(columnOrdinal);
            Board board = GetBoard(email, boardName);
            return board.GetColumnName(columnOrdinal);
        }
        /// <summary>
        /// Advances a Task to the next column
        /// </summary>
        /// <param name="email">represents the user's email</param>
        /// <param name="boardName">represents the board's name</param>
        /// <param name="taskId">represents the Task's id</param>
        /// <returns>The Task</returns>
        public ITask AdvanceTask(string email, string boardName, int columnOrdinal, int taskId)
        {
            IsAdded();
            Et.LegalColumnOrdinal(columnOrdinal);
            Board board = GetBoard(boardName, columnOrdinal, taskId);
            if (board == null)
            {
                throw new Exception("board doesn't exist");
            }
            Et.IsNotMember(board, email);
            ITask value = board.AdvanceTask(email, columnOrdinal, taskId);
            if (columnOrdinal == 0) Uh.AddTaskInProgress(email, value);
            if (columnOrdinal == 1) Uh.DeleteTask(email, taskId);
            value.Dt.Column = value.Column;
            Bc.AdvanceTask(taskId,columnOrdinal+1);
            return value;
        }


        /// <summary>
        /// Returns a list of all inProgress Tasks
        /// </summary>
        /// <param name="email">represents the user's email</param>
        /// <returns>Returns a list of all inProgress Tasks</returns>
        public List<ITask> GetInProgressList(string email)
        {
            IsAdded();
            Et.EmailNoExist(Uf, email);
            User user = Uf.GetUser(email);
            Et.UserLoggedIn(user);
            return Uh.GetTasks(email);
        }

        /// <summary>
        /// Update the task's title
        /// </summary>
        /// <param name="email">represents the user's email</param>
        /// <param name="boardName">represents the board's name</param>
        /// <param name="columnOrdinal">represents the column ordinal</param>
        /// <param name="taskId">represents the Task's id</param>
        /// <param name="newTitle">represents the Task's new title</param>
        /// <returns>The updeted task</returns>
        public ITask UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string newTitle)
        {
            IsAdded();
            Et.LegalColumnOrdinal(columnOrdinal);
            Board board = GetBoard(boardName, columnOrdinal,taskId);
            if (board == null)
            {
                throw new Exception("board doesn't exist");
            }
            ITask task = board.GetTask(taskId);
            Et.TitleLength(newTitle, task.GetTitleLimit());
            Et.IsNotMember(board, email);
            ITask taskR = board.UpdateTaskTitle(email, taskId, newTitle);
            Bc.UpdateTaskTitle(taskId, newTitle);
            return taskR;
        }

        /// <summary>
        /// Update the task's due date
        /// </summary>
        /// <param name="email">represents the user's email</param>
        /// <param name="boardName">represents the board's name</param>
        /// <param name="columnOrdinal">represents the column ordinal</param>
        /// <param name="taskId">represents the Task's id</param>
        /// <param name="newDueDate">represents the Task's new due date</param>
        /// <returns>The updeted task</returns>
        public ITask UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime newDueDate)
        {
            IsAdded();
            Et.LegalColumnOrdinal(columnOrdinal);
            Board board = GetBoard(boardName, columnOrdinal, taskId);
            if (board == null)
            {
                throw new Exception("board doesn't exist");
            }
            Et.LegalDueDate(newDueDate);
            Et.IsNotMember(board, email);
            ITask task = board.UpdateTaskDueDate(email, taskId, newDueDate);
            string newDueDateString = newDueDate.ToString("dd-MM-yyyy HH:mm:ss");
            Bc.UpdateTaskDueDate(taskId, newDueDateString);
            return task;
        }


        /// <summary>
        /// Update the task's description
        /// </summary>
        /// <param name="email">represents the user's email</param>
        /// <param name="boardName">represents the board's name</param>
        /// <param name="columnOrdinal">represents the column ordinal</param>
        /// <param name="taskId">represents the Task's id</param>
        /// <param name="newDescription">represents the Task's new description</param>
        /// <returns>The updeted task</returns>
        public ITask UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string newDescription)
        {
            IsAdded();
            Et.LegalColumnOrdinal(columnOrdinal);
            Board board = GetBoard(boardName, columnOrdinal, taskId);
            if (board == null)
            {
                throw new Exception("board doesn't exist");
            }
            Et.IsNotMember(board, email);
            ITask task = board.UpdateTaskDescription(email, taskId, newDescription);
            Bc.UpdateTaskTitle(taskId, newDescription);
            return task;
        }

        private void IsAdded()
        {
            if (Uf.addedUser)
            {
                foreach (var userEntry in Uf.users)
                {
                    string emailOfUser = userEntry.Key;

                    if (!Boards.ContainsKey(emailOfUser))
                    {
                        Boards[emailOfUser] = new List<Board>();
                        Uh.AddNewEmailToBoards(emailOfUser);
                    }
                }
                Uf.addedUser = false;
            }
        }

        private Board GetBoard(string boardName,int columnOrdinal, int taskID)
        {
            Board board = null;
            foreach ( string email in Boards.Keys)
            {
                foreach (Board b in Boards[email])
                {
                    if (b.boardName == boardName)
                    {
                        foreach (var t in b.Columns[columnOrdinal])
                        {
                            if (t.Id == taskID)
                            {
                                board = b;
                                break;
                            }
                        }
                    }
                    if (board != null) break;
                }
                if (board != null) break;
            }
            return board;
        }
    }
}
