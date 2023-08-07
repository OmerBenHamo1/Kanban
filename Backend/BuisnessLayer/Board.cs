using IntroSE.Kanban.Backend.DataAccessLayer;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    public class Board : IBoard
    {
        public DTOBoard DTOBoard { get; set; }
        public int Id {get;}
        public string boardName { get; }
        public string email { get; set; }
        public List<string> Members { get; set; }
        public List<List<ITask>> Columns { get; set; }
        public int limitTaskInColumn0 { get; set; }
        public int limitTaskInColumn1 { get; set; }
        public int limitTaskInColumn2 { get; set; }
        public Board(DTOBoard dt)
        {
            this.DTOBoard = dt;
            this.Id = dt.Id;
            this.boardName = dt.boardName;
            this.email = dt.email;
            this.Members = dt.Members;
            this.Columns = new List<List<ITask>>
            {
                new List<ITask>(),
                new List<ITask>(),
                new List<ITask>()
            };
            for(int i = 0; i < dt.Columns.Count; i++) 
            {
                foreach(var t in dt.Columns[i])
                {
                    ITask task = new Task(t);
                    this.Columns[i].Add(task);
                }
            }
            this.limitTaskInColumn0 = dt.limitTaskInColumn0;
            this.limitTaskInColumn1 = dt.limitTaskInColumn1;
            this.limitTaskInColumn2 = dt.limitTaskInColumn2;
        }
        public Board(int id, string email,string boardName, BoardController bc)
        {
            this.Id = id;
            this.email = email;
            this.boardName = boardName;
            this.Members = new List<string>();
            this.Columns = new List<List<ITask>>
            {
                new List<ITask>(),
                new List<ITask>(),
                new List<ITask>()
            };
            this.limitTaskInColumn0 = -1;
            this.limitTaskInColumn1 = -1;
            this.limitTaskInColumn2 = -1;
            this.DTOBoard = new DTOBoard(bc, 0, id, boardName, email, limitTaskInColumn0, limitTaskInColumn1, limitTaskInColumn2);
        }
        /// <summary>
        /// Adds a Task to the first column
        /// </summary>
        /// <param name="id">represents the Task's id</param>
        /// <param name="title">represents the Task's title</param>
        /// <param name="description">represents the Task's description</param>
        /// <param name="dueDate">represents the Task's dueDate</param>
        /// <returns>The added Task</returns>
        public ITask AddTask(int id,string title, string description, DateTime dueDate, TaskController Tc)
        {
            if (dueDate < DateTime.Now)
                throw new Exception("the duedate is not possible");
            if (this.limitTaskInColumn0 == this.Columns[0].Count())//access DB to check number of tasks)
                throw new Exception("The column 'backLog'  doesn't have anymore space!");
            Task task = new(id, DateTime.Now, dueDate, title, description, this.Id, Tc);
            this.Columns[0].Add(task);
            return task;
        }
        /// <summary>
        /// assigns a user to a task
        /// </summary>
        /// <param name="email">represents the email of the board owner/ task assigne</param>
        /// <param name="columnOrdinal">represents the column number</param>
        /// <param name="columnOrdinal">represents the task id</param>
        /// <param name="emailAssignee">represents the user to assign the task to</param>
        /// <returns>void, or error if accured</returns>
        public ITask AssignTask(string email, int columnOrdinal, int taskID, string emailAssignee)
        {
            ITask value = null;
            foreach (ITask t in this.Columns[columnOrdinal])
            {
                if (t.Id == taskID)
                {
                    value = t;
                    break;
                }
            }
            if (value == null)
                throw new Exception("Task doesn't exist");
            if(value.Assignee != null)
            {
                if (value.Assignee != email)
                    throw new Exception("only the task's assigne can change the assigne");
                else
                    value.AssignTask(emailAssignee);
            }
            else
            {
                value.AssignTask(emailAssignee);
            }
            return value;
        }
        /// <summary>
        /// returns a column
        /// </summary>
        /// <param name="columnOrdinal">represents the column number</param>
        /// <returns>the column</returns>
        public List<ITask> GetColumn(int columnOrdinal)
        {
            return this.Columns[columnOrdinal];
        }
        /// <summary>
        /// Unassigns the tasks of the user that left the board
        /// </summary>
        /// <param name="email">represents the user's email</param>
        /// <returns>void, unless an error accured</returns>
        public void UnAssign(string email)
        {
            for (int i = 0; i < this.Columns.Count() - 1; i++)
            {
                foreach (var t in this.Columns[i])
                {
                    if (t.Assignee == email)
                        t.UnAssignee();
                }
            }
        }
        /// <summary>
        /// Deletes a Task
        /// </summary>
        /// <param name="email">represents the assigne's email</param>
        /// <param name="taskId">represents the Task's id</param>
        /// <returns>A text about the deleted Task</returns>
        public ITask DeleteTask(string email,int taskId)
        {
            ITask task = null;
            int i = 0;
            while (i < this.Columns.Count())
            {
                foreach (ITask t in this.Columns[i])
                {
                    if (t.Id == taskId)
                    {
                        task = t;
                        break;
                    }
                }
                if (task != null) break;
                i++;
            }
            if (task == null) 
                throw new Exception("Task does not exists!");
            if (task.Assignee != email)
                throw new Exception("a task can be deleted only by it's assigne");
            this.Columns[i].Remove(task);
            return task;
        }
        /// <summary>
        /// Advances a Task
        /// </summary>
        /// <param name="taskId">represents the Task's id</param>
        /// <param name="columnOrdinal">represents the column</param>
        /// <returns>Two values: the Task</returns>
        public ITask AdvanceTask(string email,int columnOrdinal,int taskId)
        {
            ITask value = null;
            foreach (ITask t in this.Columns[columnOrdinal])
            {
                if (t.Id == taskId)
                {
                    value = t;
                    break;
                }
            }
            if (value == null)
                throw new Exception($"Task {taskId} doesn't exist");
            if (value.Assignee != email)
                throw new Exception("a task can be advanced only by it's assigne");
            value.AdvanceTask();
            this.Columns[columnOrdinal].Remove(value);
            this.Columns[columnOrdinal + 1].Add(value);
            return value;
        }
        /// <summary>
        /// Updates the number of Tasks possible in a specific column
        /// </summary>
        /// <param name="columnOrdinal">represents the column</param>
        /// <param name="limitTaskInColumn">represents the new limit</param>
        /// <returns>The new limit</returns>
        public int LimitColumn(int columnOrdinal, int limitTaskInColumn)
        { 
            if (this.Columns[columnOrdinal].Count() > limitTaskInColumn)
                throw new Exception("There are too many Tasks in column " + GetColumnName(columnOrdinal));
            if (limitTaskInColumn == 0) limitTaskInColumn0 = limitTaskInColumn;
            if (limitTaskInColumn == 1) limitTaskInColumn1 = limitTaskInColumn;
            if (limitTaskInColumn == 2) limitTaskInColumn2 = limitTaskInColumn;
            return limitTaskInColumn;
        }
        /// <summary>
        /// Returns the required Task
        /// </summary>
        /// <param name="TaskId">represents the Task's id</param>
        /// <returns>The Task</returns>
        public ITask GetTask( int TaskId)
        {
            ITask value = null;
            int i = 0;
            while (i < this.Columns.Count)
            {
                foreach (ITask t in this.Columns[i])
                {
                    if (t.Id == TaskId)
                    {
                        value = t;
                        break;
                    }
                }
                i++;
                if (value != null) break;
            }
            if (value == null) throw new Exception("The Task doesn't exist!");
            return value;
        }
        /// <summary>
        /// Returns the limit of a column
        /// </summary>
        /// <param name="columnOrdinal">represents the column</param>
        /// <returns>The limit of the column</returns>
        public int GetColumnLimit(int columnOrdinal)
        {
            if (columnOrdinal == 0) return limitTaskInColumn0;
            else if (columnOrdinal == 1) return limitTaskInColumn1;
            else return limitTaskInColumn2;
        }
        /// <summary>
        /// Returns the name of a column
        /// </summary>
        /// <param name="columnOrdinal">represents the column</param>
        /// <returns>The name of the column</returns>
        public string GetColumnName(int columnOrdinal)
        {
            if (columnOrdinal == 0) return "backlog";
            else if (columnOrdinal == 1) return "in progress";
            else return "done";
        }

        /// <summary>
        /// Returns the required Task
        /// </summary>
        /// <param name="email">represents the assigne's email</param>
        /// <param name="taskId">represents the Task's id</param>
        /// <param name="newDueDate">represents the new due date</param>
        /// <returns>The updated task</returns>
        public ITask UpdateTaskDueDate(string email, int taskId, DateTime newDueDate)
        {
            if (newDueDate < DateTime.Now)
                throw new Exception("the due date is not possible");
            ITask value = null;
            int i = 0;
            while (i < this.Columns.Count)
            {
                foreach (ITask t in this.Columns[i])
                {
                    if (t.Id == taskId)
                    {
                        value = t;
                        break;
                    }
                }
                if (value != null) break;
                i++;
            }
            if (value == null) throw new Exception("The Task doesn't exist!");
            if (value.Assignee != email)
                throw new Exception("a task can be updated only by it's assigne");
            value.UpdateTaskDueDate(newDueDate);
            return value;
        }


        /// <summary>
        /// Returns the required Task
        /// </summary>
        /// <param name="email">represents the assigne's email</param>
        /// <param name="taskId">represents the Task's id</param>
        /// <param name="newTitle">represents the new title</param>
        /// <returns>The updated task</returns>
        public ITask UpdateTaskTitle(string email, int taskId, string newTitle)
        {
            ITask value = null;
            int i = 0;
            while (i < this.Columns.Count)
            {
                foreach (ITask t in this.Columns[i])
                {
                    if (t.Id == taskId)
                    {
                        value = t;
                        break;
                    }
                }
                if (value != null) break;
                i++;
            }
            if (value == null) throw new Exception("The Task doesn't exist!");
            if (value.Assignee != email)
                throw new Exception("a task can be updated only by it's assigne");
            value.UpdateTaskTitle(newTitle);
            return value;
        }

        /// <summary>
        /// Returns the required Task
        /// </summary>
        /// <param name="email">represents the assigne's email</param>
        /// <param name="taskId">represents the Task's id</param>
        /// <param name="newDescription">represents the new description</param>
        /// <returns>The updated task</returns>
        public ITask UpdateTaskDescription(string email, int taskId, string newDescription)
        {
            ITask value = null;
            int i = 0;
            while (i < this.Columns.Count)
            {
                foreach (ITask t in this.Columns[i])
                {
                    if (t.Id == taskId)
                    {
                        value = t;
                        break;
                    }
                }
                if (value != null) break;
                i++;
            }
            if (value == null) throw new Exception("The Task doesn't exist!");
            if (value.Assignee != email)
                throw new Exception("a task can be updated only by it's assigne");
            value.UpdateTaskDescription(newDescription);
            return value;
        }
    }
}
