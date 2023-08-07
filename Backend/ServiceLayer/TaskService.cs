using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BuisnessLayer;
using System.Text.Json;
using System.Text.Json.Serialization;
using Task = IntroSE.Kanban.Backend.BuisnessLayer.Task;
using Microsoft.VisualBasic;
using log4net;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class TaskService : ITaskService
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public IBoardFacade bf { get; set; }
        public TaskService(IBoardFacade _bf)
        {
            this.bf = _bf;
        }

        ///<summery>
        ///Adding a new task to backlog column
        ///</summery>
        ///<param name= "email"> The user's email address, it is used as a username login to the system.</param>
        ///<param name = "boardName" > The name of the board to add the task.</param>
        ///<param name = "title" > The list title.</param>
        ///<param name = "description" > The task discripion.</param>        
        ///<param name = "dueDate" > The due date for the task.</param>
        ///<returns>JSON object represents the new task, or an error if accured.</returns>
        public string AddTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
            try
            {
                Task task = (Task)bf.AddTask(email, boardName, title, description, dueDate);
                TaskToSend taskToSend = new(task);
                Response response = new(taskToSend, null);
                Log.Info($" Task {taskToSend.Id} was created for user: {email}.");
                return JsonSerializer.Serialize(response);
            }
            catch (Exception e)
            {
                Response response = new(null, e.Message);
                string json = JsonSerializer.Serialize<Response>(response);
                Log.Error($"Tried to create a Task. user {email} , boardName: {boardName}. Error: {e.Message}");
                return json;
            }
        }
        ///<summery>
        ///Deletes a Task
        ///</summery>
        ///<param name= "email"> The user's email address, it is used as a username login to the system.</param>
        ///<param name = "boardName" > The name of the board to add the task.</param>
        ///<param name = "taskId" > The Task's id.</param>
        ///<returns>JSON object represents a text about the deleted Task, or an error if accured.</returns>
        public string DeleteTask(string email, string boardName, int taskId)
        {
            try
            {
                string s =  bf.DeleteTask(email, boardName, taskId);
                Response response = new(s, null);
                Log.Info($" Task {taskId} was deleted for user: {email}.");
                return JsonSerializer.Serialize(response);
            }
            catch (Exception e)
            {
                Response response = new(null, e.Message);
                string json = JsonSerializer.Serialize<Response>(response);
                Log.Error($"Tried to delete a Task. user {email} , boardName: {boardName}. Error: {e.Message}");
                return json;
            }
        }

        ///<summery>
        ///Adding a new task to backlog column
        ///</summery>
        ///<param name= "email"> The user's email address, it is used as a username login to the system.</param>
        ///<param name = "boardName" > The name of the board to get the task from.</param>
        ///<param name = "taskId" > the task id.</param>
        ///<returns>JSON object represents the task we want to get, or an error if accured.</returns>
        public string GetTask(string email, string boardName, int taskId)
        {
            try
            {
                Task task = (Task)bf.GetTask(email, boardName, taskId);
                TaskToSend taskToSend = new(task);
                Response response = new(taskToSend, null);
                Log.Info($" Get Task {taskToSend.Id} was successful for the user: {email}.");
                return JsonSerializer.Serialize(response);
            }
            catch (Exception e)
            {
                Response response = new(null, e.Message);
                string json = JsonSerializer.Serialize<Response>(response);
                Log.Error($"Tried to get a Task. user {email} , boardName: {boardName}. Error: {e.Message}");
                return json;
            }
        }
        ///<summery>
        ///Adding a new task to backlog column
        ///</summery>
        ///<param name = "email" > The user's email address, it is used as a username login to the system.</param>
        ///<param name = "boardName" > The name of the board to get the task from.</param>
        ///<param name = "columnOrdinal" > The column ordinal.</param>
        ///<param name = "taskId" > The task id of the task we want to advace.</param>
        ///<returns>JSON object represents the updated task with the updeted state, or an error if accured.</returns>
        public string AdvanceTask(string email, string boardName,int columnOrdinal, int taskId)
        {
            try
            {
                Task task = (Task)bf.AdvanceTask(email, boardName, columnOrdinal,taskId);
                TaskToSend taskToSend = new(task);
                Response response = new(taskToSend, null);
                Log.Info($"Task {taskToSend.Id} was advance for user: {email}.");
                return JsonSerializer.Serialize(response);
            }
            catch (Exception e)
            {
                Response response = new(null, e.Message);
                string json = JsonSerializer.Serialize<Response>(response);
                Log.Error($"Tried to advance a Task. user {email} , boardName: {boardName}. Error: {e.Message}");
                return json;
            }
        }

        ///<summery>
        ///Adding a new task to backlog column
        ///</summery>
        ///<param name= "email"> The user's email address, it is used as a username login to the system.</param>
        ///<param name = "boardName" > The name of the board to get the task from.</param>
        /// <param name="columnOrdinal"> The column ordinal</param>
        ///<param name = "taskId" > the task id of the task we want to advace.</param>
        ///<param name = "newDueDate" > the new due date.</param>
        ///<returns>JSON object represents the updated task with the new due date, or an error if accured.</returns>
        public string UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime newDueDate)
        {
            try
            {
                Task task = (Task)bf.UpdateTaskDueDate(email, boardName, columnOrdinal, taskId, newDueDate);
                TaskToSend taskToSend = new(task);
                Response response = new(taskToSend, null);
                Log.Info($"In task: {taskToSend.Id}, the due date was updated for user: {email}.");
                return JsonSerializer.Serialize(response);
            }
            catch (Exception e)
            {
                Response response = new(null, e.Message);
                string json = JsonSerializer.Serialize<Response>(response);
                Log.Error($"Tried to updaet the due date in a Task. user {email} , boardName: {boardName}. Error: {e.Message}");
                return json;
            }
        }

        ///<summery>
        ///Adding a new task to backlog column
        ///</summery>
        ///<param name= "email"> The user's email address, it is used as a username login to the system.</param>
        ///<param name = "boardName" > The name of the board to get the task from.</param>        
        /// <param name="columnOrdinal"> The column ordinal</param>
        ///<param name = "taskId" > the task id of the task we want to advace.</param>
        ///<param name = "newTitle" > the new title.</param>
        ///<returns>JSON object represents the updated task with the new title, or an error if accured.</returns>
        public string UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string newTitle)
        {
            try
            {
                Task task = (Task)bf.UpdateTaskTitle(email, boardName, columnOrdinal, taskId, newTitle);
                TaskToSend taskToSend = new(task);
                Response response = new(taskToSend, null);
                Log.Info($"In task: {taskToSend.Id}, the title was updated for user: {email}.");
                return JsonSerializer.Serialize(response);
            }
            catch (Exception e)
            {
                Response response = new(null, e.Message);
                string json = JsonSerializer.Serialize<Response>(response);
                Log.Error($"Tried to updaet the title in a Task. user {email} , boardName: {boardName}. Error: {e.Message}");
                return json;
            }
        }

        ///<summery>
        ///Adding a new task to backlog column
        ///</summery>
        ///<param name= "email"> The user's email address, it is used as a username login to the system.</param>
        ///<param name = "boardName" > The name of the board to get the task from.</param>
        /// <param name="columnOrdinal"> The column ordinal</param>
        ///<param name = "taskId" > the task id of the task we want to advace.</param>
        ///<param name = "newDescription" > the new description.</param>
        ///<returns>JSON object represents the updated task with the new description, or an error if accured.</returns>
        public string UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string newDescription)
        {
            try
            {
                Task task = (Task)bf.UpdateTaskTitle(email, boardName, columnOrdinal, taskId, newDescription);
                TaskToSend taskToSend = new(task);
                Response response = new(taskToSend, null);
                Log.Info($"In task: {taskToSend.Id}, the description was updated for user: {email}.");
                return JsonSerializer.Serialize(response);
            }
            catch (Exception e)
            {
                Response response = new(null, e.Message);
                string json = JsonSerializer.Serialize<Response>(response);
                Log.Error($"Tried to updaet the title in a Task. user {email} , boardName: {boardName}. Error: {e.Message}");
                return json;
            }
        }

        ///<summery>
        ///Creating a list of the Tasks taht are in progress for a user
        ///</summery>
        ///<param name= "email"> The user's email address, it is used as a username login to the system.</param>
        ///<returns>JSON object represents the list of Tasks, or an error if accured.</returns>
        public string InProgressTasks(string email)
        {
            try
            {
                List<ITask> success = bf.GetInProgressList(email);
                List<TaskToSend> tasksToSend = new List<TaskToSend>();
                foreach (ITask task in success) {
                    tasksToSend.Add(new TaskToSend(task));
                }
                Response response = new(tasksToSend, null);
                Log.Info($"InProgressTasks was success for user: {email}.");
                return JsonSerializer.Serialize(response);
            }
            catch (Exception e)
            {
                Response response = new(null, e.Message);
                string json = JsonSerializer.Serialize<Response>(response);
                Log.Error($"Tried to InProgressTasks, but failed. user {email}. Error: {e.Message}");
                return json;
            }
        }
        /// <summary>
        /// This method transfers a board ownership.
        /// </summary>
        /// <param name="currentOwnerEmail">Email of the current owner. Must be logged in</param>
        /// <param name="newOwnerEmail">Email of the new owner</param>
        /// <param name="boardName">The name of the board</param>
        /// <returns>An empty response, unless an error occurs</returns>
        public string TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            try
            {
                bf.TransferOwnership(currentOwnerEmail, newOwnerEmail, boardName);
                Response response = new(null, null);
                Log.Info($"TransferOwnership was success for currentOwnerEmail: {currentOwnerEmail}, to newOwnerEmail: {newOwnerEmail} in board {boardName}.");
                return JsonSerializer.Serialize(response);
            }
            catch (Exception e)
            {
                Response response = new(null, e.Message);
                string json = JsonSerializer.Serialize<Response>(response);
                Log.Error($"Tried to TransferOwnership, but failed. currentOwnerEmail {currentOwnerEmail}, newOwnerEmail{newOwnerEmail},boardName{boardName} . Error: {e.Message}");
                return json;
            }
        }
        /// <summary>
        /// This method assigns a task to a user
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column number. The first column is 0, the number increases by 1 for each column</param>
        /// <param name="taskID">The task to be updated identified a task ID</param>        
        /// <param name="emailAssignee">Email of the asignee user</param>
        /// <returns>An empty response, unless an error occurs</returns>
        public string AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
        {
            try
            {
                bf.AssignTask(email, boardName, columnOrdinal, taskID, emailAssignee);
                Response response = new(null, null);
                Log.Info($"AssignTask was success for currentOwnerEmail: {email}, to emailAssignee: {emailAssignee}");
                return JsonSerializer.Serialize(response);
            }
            catch (Exception e)
            {
                Response response = new(null, e.Message);
                string json = JsonSerializer.Serialize<Response>(response);
                Log.Error($"Tried to AssignTask, but failed. currentOwnerEmail {email}, emailAssignee: {emailAssignee},boardName{boardName} . Error: {e.Message}");
                return json;
            }
        }
    }
}
