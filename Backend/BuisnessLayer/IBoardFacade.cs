using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    public interface IBoardFacade
    {
        Dictionary<string, List<Board>> Boards { get; set; }
        IUserFacade Uf { get;}
        IUserHistory Uh { get; }
        int numberOfTaskTotal { get; set; }
        void LoadData();
        void DeleteData();
        Board CreateBoard(string email, string boardName);
        string DeleteBoard(string email, string boardName);
        Board GetBoard(string email, string boardName);
        Board GetBoard(int boardID);
        int LimitColumn(string email, string boardName, int columnOrdinal, int limit);
        int GetColumnLimit(string email, string boardName, int columnOrdinal);
        string GetColumnName(string email, string boardName, int columnOrdinal);
        List<ITask> GetColumn(string email, string boardName, int columnOrdinal);
        List<int> GetUserBoards(string email);
        void JoinBoard(string email, int boardID);
        void LeaveBoard(string email, int boardID);
        string GetBoardName(int boardId);
        ITask AddTask(string email, string boardName, string title, string description, DateTime dueDate);
        ITask GetTask(string email, string boardName, int id);
        ITask AdvanceTask(string email, string boardName, int columnOrdinal, int taskId);
        ITask UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string newTitle);
        ITask UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime newDueDate);
        ITask UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string description);
        List<ITask> GetInProgressList(string email);
        string DeleteTask(string email, string boardName, int taskId);
        void TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName);
        void AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee);
    }
}
