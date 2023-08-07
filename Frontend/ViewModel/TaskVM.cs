using IntroSE.Kanban.Frontend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Frontend.ViewModel
{
    public class TaskVM : NotifiableObject
    {
        private StartFrontend StartFrontend { get; }
        public TaskVM(StartFrontend startFrontend, TaskUI taskUI)
        {
            StartFrontend = startFrontend;
            this.TaskUI = taskUI;
        }


        private TaskUI? taskUI;
        public TaskUI? TaskUI
        {
            get { return taskUI; }
            set
            {
                taskUI = value;
                RaisePropertyChanged("TaskUI");
            }
        }
    }
}
