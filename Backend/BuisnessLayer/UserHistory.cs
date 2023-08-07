using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer;


namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    public class UserHistory : IUserHistory
    {
        public Dictionary<string, List<ITask>> TasksInProgress { get; set; }
        public Dictionary<string, List<int>> BoardsOfUsers { get; set; }

        public UserHistory() {
            this.TasksInProgress = new Dictionary<string, List<ITask>>();
            this.BoardsOfUsers = new Dictionary<string, List<int>>();
        }
        public List<ITask> GetTasks(string email)
        {
            return this.TasksInProgress[email];
        }
        public List<int> GetBoardsOfUser(string email)
        {
            return this.BoardsOfUsers[email];
        }
        public void AddTaskInProgress(string email, ITask task)
        {
            if (this.TasksInProgress.ContainsKey(task.Assignee))
            {
                if (!this.TasksInProgress[task.Assignee].Contains(task))
                    this.TasksInProgress[task.Assignee].Add(task);
            }
            else
                this.TasksInProgress[task.Assignee] = new List<ITask> { task };
        }
        public void AddBoardToOwner(string email,Board board)
        {
            if (this.BoardsOfUsers.ContainsKey(email))
            {
                if (!this.BoardsOfUsers[email].Contains(board.Id))
                    this.BoardsOfUsers[email].Add(board.Id);
            }
            else
                this.BoardsOfUsers[email] = new List<int> { board.Id};
        }
        public void AddNewEmailToBoards(string email)
        {
            if(!this.BoardsOfUsers.ContainsKey(email))
                this.BoardsOfUsers[email] = new List<int>();
        }
        public void DeleteTask(string email,int id)
        {
            //only remove from list, not to actualy delete!!!
            ITask desiredTask = this.TasksInProgress[email].Find(t => t.Id == id);
            this.TasksInProgress[email].Remove(desiredTask);
        }
        public void DeleteData()
        {
            this.TasksInProgress.Clear();
            this.BoardsOfUsers.Clear();
        }
    }
}
