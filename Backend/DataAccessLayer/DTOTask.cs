using IntroSE.Kanban.Backend.BuisnessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class DTOTask
    {
        private TaskController controller { get; set; }
        internal int isPersisted { get; set; } // 0-false, 1-true
        internal int TaskId { get; }
        internal string Title { get; set; }
        internal string Description { get; set; }
        internal string DueDate { get; set; }
        internal string CreationTime { get; }
        internal string Assignee { get; set; }
        internal int BoardID { get; set; }
        internal int Column { get; set; }
        public DTOTask(TaskController controller, int isPersisted, int id, string title, string description, string dueDate, string creationTime, string assignee, int boardID, int column)
        {
            this.controller = controller;
            this.isPersisted = isPersisted;
            TaskId = id;
            Title = title;
            Description = description;
            DueDate = dueDate;
            CreationTime = creationTime;
            Assignee = assignee;
            BoardID = boardID;
            Column = column;
        }

        public ITask AddTask()
        {
            throw new NotImplementedException();
        }

        public ITask DeleteTask()
        {
            throw new NotImplementedException();
        }
    }
}
