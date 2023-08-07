using IntroSE.Kanban.Backend.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    public interface IBoard
    {
        int Id { get; }
        string boardName { get;}
        string email { get; set; }
        List<string> Members { get; set; }
        List<List<ITask>> Columns { get; set; }
        int limitTaskInColumn0 { get; set; }
        int limitTaskInColumn1 { get; set; }
        int limitTaskInColumn2 { get; set; }
        ITask AddTask(int id, string title, string description, DateTime dueDate, TaskController Tc);
        ITask AssignTask(string email, int columnOrdinal, int taskID, string emailAssignee);
        void UnAssign(string email);
        List<ITask> GetColumn(int columnOrdinal);
        int LimitColumn(int columnOrdinal, int  limitTaskInColumn);
        ITask AdvanceTask(string email, int columnOrdinal, int taskId);
        ITask GetTask(int TaskId);
        int GetColumnLimit(int columnOrdinal);
        string GetColumnName(int columnOrdinal);
        ITask UpdateTaskDueDate(string email,int taskId, DateTime newDueDate);
        ITask UpdateTaskTitle(string email, int taskId, string newTitle);
        ITask UpdateTaskDescription(string email, int taskId, string newDescription);
        ITask DeleteTask(string email, int taskId);
    }
}
