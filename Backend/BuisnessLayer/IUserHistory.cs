using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    public interface IUserHistory
    {
        Dictionary<string , List<ITask>> TasksInProgress { get; set; }
        Dictionary<string,List<int>> BoardsOfUsers { get; set; }
        void AddTaskInProgress(string email, ITask task);
        void AddBoardToOwner(string email, Board board);
        void DeleteTask(string email, int id);
        List<ITask> GetTasks(string email);
        List<int> GetBoardsOfUser(string email);
        void AddNewEmailToBoards(string email);
        void DeleteData();
    }
}