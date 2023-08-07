using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BuisnessLayer;
using Microsoft.VisualBasic;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace IntroSE.Kanban.Backend.ServiceLayer
{
    [Serializable]
    public class TaskToSend : ITaskToSend
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreationTime { get; set; }
        public string Assignee { get; set; }
        [JsonConstructor]
        public TaskToSend(int Id, string Title, string Description, DateTime DueDate, DateTime CreationTime, string Assignee)
        {
            this.Id = Id;
            this.Title = Title;
            this.Description = Description;
            this.DueDate = DueDate;
            this.CreationTime = CreationTime;
            this.Assignee = Assignee;
        }
        public TaskToSend(ITask task)
        {
            this.Id = task.Id;
            this.Title = task.Title;
            this.Description = task.Description;
            this.DueDate = task.DueDate;
            this.CreationTime = task.CreationTime;
            this.Assignee = task.Assignee;
        }
    }
}
