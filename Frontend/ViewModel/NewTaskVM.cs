using IntroSE.Kanban.Backend.BuisnessLayer;
using IntroSE.Kanban.Frontend.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Frontend.ViewModel
{
    internal class NewTaskVM : NotifiableObject
    {
        private StartFrontend StartFrontend;
        public NewTaskVM(StartFrontend startFrontend)
        {
            StartFrontend = startFrontend;
        }
    }
}
