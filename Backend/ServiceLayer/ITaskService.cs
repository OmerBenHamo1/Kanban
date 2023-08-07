using IntroSE.Kanban.Backend.BuisnessLayer;
using Microsoft.VisualBasic;
using System;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public interface ITaskService
    {
        IBoardFacade bf { get; set; }
        string AddTask(string email, string boardName, string title, string description, DateTime dueDate);
        string GetTask(string email, string boardName, int id);
        string AdvanceTask(string email, string boardName, int columnOrdinal,int taskId);
        string UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime newDueDate);
        string UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string newTitle);
        string UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string newDescription);
        string InProgressTasks(string email);
        string DeleteTask(string email, string boardName, int taskId);
        string TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName);
        string AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee);
    }
}