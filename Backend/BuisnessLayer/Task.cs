using IntroSE.Kanban.Backend.DataAccessLayer;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    public class Task : ITask
    {
        public DTOTask Dt {get; set;}
        public int Id { get; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreationTime { get; }
        public string Assignee { get; set; }
        public int BoardId { get; set; }
        public int Column { get; set; }

        private const int titleLimit = 50;
        public Task(int taskId, DateTime creationTime, DateTime dueDate, string title, string description,int boardId, TaskController _tc) // DTOTask _dt, 
        {
            this.Id = taskId;
            this.CreationTime = creationTime;
            this.DueDate = dueDate;
            this.Title = title;
            this.Description = description;
            this.Assignee = null;
            this.BoardId = boardId;
            this.Column = 0;
            string dueDateString = dueDate.ToString("yyyy-MM-dd HH:mm:ss");
            string creationTimeString = creationTime.ToString("yyyy-MM-dd HH:mm:ss"); 
            this.Dt = new DTOTask(_tc, 0, taskId, title, description, dueDateString, creationTimeString, this.Assignee, BoardId, Column);
        }
        public Task(DTOTask dt)
        {
            this.Dt = dt;
            this.Id = dt.TaskId;
            this.Title = dt.Title;
            this.Description = dt.Description;
            DateTime dueDate = DateTime.Parse(dt.DueDate);
            DateTime creationTime = DateTime.Parse(dt.CreationTime);
            this.DueDate= dueDate;
            this.CreationTime= creationTime;
            this.Assignee= dt.Assignee;
            this.BoardId = dt.BoardID; 
            this.Column = dt.Column;

        }

        public int GetTitleLimit()
        {
            return titleLimit;
        }
        public void AssignTask(string emailAssignee)
        {
            this.Assignee = emailAssignee;
            this.Dt.Assignee = emailAssignee;
        }
        public void UnAssignee()
        {
            this.Assignee = null;
        }
        public void AdvanceTask()
        {
            this.Column += 1;
            this.Dt.Column += 1;
        }
        public void UpdateTaskDueDate(DateTime newDueDate)
        {
            this.DueDate = newDueDate;
            this.Dt.DueDate = newDueDate.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public void UpdateTaskTitle(string newTitle)
        {
            this.Title = newTitle;
            this.Dt.Title = newTitle;
        }
        public void UpdateTaskDescription(string newDescription)
        {
            this.Description = newDescription;
            this.Dt.Description = newDescription;
        }
    }
}
