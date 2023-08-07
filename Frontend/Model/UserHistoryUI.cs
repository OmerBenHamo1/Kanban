using System.Collections.Generic;

namespace IntroSE.Kanban.Frontend.Model
{
    public class UserHistoryUI: NotifiableModelObject
    {
        public Dictionary<string, List<TaskUI>> TasksInProgress { get; set; }
        public Dictionary<string, List<int>> BoardsOfUsers { get; set; }
        public UserHistoryUI(Dictionary<string, List<TaskUI>> tasksInProgress, Dictionary<string, List<int>> boardsOfUsers, BackendController backendController) : base(backendController)
        {
            TasksInProgress = tasksInProgress;
            BoardsOfUsers = boardsOfUsers;
        }
    }
}
