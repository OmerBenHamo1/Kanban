using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using IntroSE.Kanban.Frontend.Model;
using IntroSE.Kanban.Backend.ServiceLayer;
using Microsoft.VisualBasic;

namespace IntroSE.Kanban.Frontend.Model
{
    public class TaskFacadeUI: NotifiableModelObject
    {
        public TaskFacadeUI(BackendController backendController): base(backendController) { }
        public TaskUI AddTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
            string Json = this.Controller.St.ts.AddTask(email, boardName, title, description, dueDate);
            var response = JsonSerializer.Deserialize<Response>(Json);
            if (response == null)
                throw new Exception("response is null");
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);
            TaskToSend taskToSend = JsonSerializer.Deserialize<TaskToSend>((JsonElement)response.ReturnValue);
            return new TaskUI(taskToSend, this.Controller);
        }
        public string DeleteTask(string email, string boardName, int taskId)
        {
            string Json = this.Controller.St.ts.DeleteTask(email, boardName, taskId);
            var response = JsonSerializer.Deserialize<Response>(Json);
            if (response == null)
                throw new Exception("response is null");
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);
            return JsonSerializer.Deserialize<string>( (JsonElement)response.ReturnValue);
        }
        public TaskUI GetTask(string email, string boardName, int taskId)
        {
            string Json = this.Controller.St.ts.GetTask(email, boardName, taskId);
            var response = JsonSerializer.Deserialize<Response>(Json);
            if (response == null)
                throw new Exception("response is null");
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);
            var taskToSend = JsonSerializer.Deserialize<TaskToSend>((JsonElement) response.ReturnValue);
            return new TaskUI(taskToSend, this.Controller);
        }
        public TaskUI AdvanceTask(string email, string boardName, int columnOrdinal, int taskId)
        {
            string Json = this.Controller.St.ts.AdvanceTask(email, boardName,columnOrdinal, taskId);
            var response = JsonSerializer.Deserialize<Response>(Json);
            if (response == null)
                throw new Exception("response is null");
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);
            TaskToSend taskToSend = JsonSerializer.Deserialize<TaskToSend>((JsonElement)response.ReturnValue);
            return new TaskUI(taskToSend, this.Controller);
        }
        public TaskUI UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string newDescription)
        {
            string Json = this.Controller.St.ts.UpdateTaskDescription(email, boardName, columnOrdinal, taskId, newDescription);
            var response = JsonSerializer.Deserialize<Response>(Json);
            if (response == null)
                throw new Exception("response is null");
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);
            TaskToSend taskToSend = JsonSerializer.Deserialize<TaskToSend>((JsonElement)response.ReturnValue);
            return new TaskUI(taskToSend, this.Controller);
        }
        public TaskUI UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime newDueDate)
        {
            string Json = this.Controller.St.ts.UpdateTaskDueDate(email, boardName, columnOrdinal, taskId, newDueDate);
            var response = JsonSerializer.Deserialize<Response>(Json);
            if (response == null)
                throw new Exception("response is null");
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);
            TaskToSend taskToSend = JsonSerializer.Deserialize<TaskToSend>((JsonElement)response.ReturnValue);
            return new TaskUI(taskToSend, this.Controller);
        }
        public TaskUI UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string newTitle)
        {
            string Json = this.Controller.St.ts.UpdateTaskTitle(email, boardName, columnOrdinal, taskId, newTitle);
            var response = JsonSerializer.Deserialize<Response>(Json);
            if (response == null)
                throw new Exception("response is null");
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);
            TaskToSend taskToSend = JsonSerializer.Deserialize<TaskToSend>((JsonElement)response.ReturnValue);
            return new TaskUI(taskToSend, this.Controller);
        }
        public List<TaskUI> InProgressTasks(string email)
        {
            string Json = this.Controller.St.ts.InProgressTasks(email);
            var response = JsonSerializer.Deserialize<Response>(Json);
            if (response == null)
                throw new Exception("response is null");
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);
            List<TaskUI> inProgressTasks = new List<TaskUI>();
            List<TaskToSend> taskToSends = JsonSerializer.Deserialize<List<TaskToSend>>((JsonElement)response.ReturnValue);
            foreach (var t in taskToSends)
            {
                inProgressTasks.Add(new TaskUI((TaskToSend)t, this.Controller));
            }
            return inProgressTasks;
        }
        public string TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            string Json = this.Controller.St.ts.TransferOwnership(currentOwnerEmail, newOwnerEmail, boardName);
            var response = JsonSerializer.Deserialize<Response>(Json);
            if (response == null)
                throw new Exception("response is null");
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);
            return "The transfer was a success";
        }
        public string AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
        {
            string Json = this.Controller.St.ts.AssignTask(email, boardName, columnOrdinal,taskID,emailAssignee);
            var response = JsonSerializer.Deserialize<Response>(Json);
            if (response == null)
                throw new Exception("response is null");
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);
            return "The assign was a success";
        }
    }
}
