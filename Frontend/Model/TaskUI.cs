using IntroSE.Kanban.Backend.ServiceLayer;
using IntroSE.Kanban.Frontend.Model;
using System;

namespace IntroSE.Kanban.Frontend.Model
{
    public class TaskUI: NotifiableModelObject
    {
        public int Id { get; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreationTime { get; }
        public string Assignee { get; }
        public TaskUI(TaskToSend taskToSend, BackendController backendController) : base(backendController)
        {
            Id = taskToSend.Id;
            Title = taskToSend.Title;
            Description = taskToSend.Description;
            DueDate = taskToSend.DueDate;
            CreationTime = taskToSend.CreationTime;
            Assignee = taskToSend.Assignee;
        }
    }
}
