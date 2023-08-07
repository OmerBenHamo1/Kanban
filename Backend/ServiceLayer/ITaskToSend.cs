using System;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public interface ITaskToSend
    {
        string Assignee { get; set; }
        DateTime CreationTime { get; set; }
        string Description { get; set; }
        DateTime DueDate { get; set; }
        int Id { get; set; }
        string Title { get; set; }
    }
}