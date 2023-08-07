using IntroSE.Kanban.Frontend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Frontend.ViewModel
{
    public class InProgressTasksVM: NotifiableObject
    {
        private StartFrontend StartFrontend { get;}
        public InProgressTasksVM(StartFrontend startFrontend, List<TaskUI> inProgressTasks)
        {
            InProgressTasks = inProgressTasks;
            this.StartFrontend = startFrontend;
        }
        private List<TaskUI>? inProgressTasks;
        public List<TaskUI>? InProgressTasks
        {
            get { return inProgressTasks; }
            set
            {
                inProgressTasks = value;
                RaisePropertyChanged("InProgressTasks");
            }
        }
    }
}
