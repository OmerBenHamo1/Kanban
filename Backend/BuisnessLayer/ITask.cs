using IntroSE.Kanban.Backend.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    public interface ITask
    {
        DTOTask Dt { get; set; }
        int Id { get; }
        string Title { get; set; }
        string Description { get; set; }
        DateTime DueDate { get; set; }
        DateTime CreationTime { get; }
        string Assignee { get; set; }
        int BoardId { get; set; }
        int Column { get; set; }
        void AssignTask(string emailAssignee);
        void UnAssignee();
        void AdvanceTask();
        void UpdateTaskDueDate(DateTime newDueDate);
        void UpdateTaskTitle(string newTitle);
        void UpdateTaskDescription(string newDescription);
        int GetTitleLimit();
    }

}
