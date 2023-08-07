using IntroSE.Kanban.Frontend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Frontend
{
    public class StartFrontend
    {
        public BoardFacadeUI BoardFacadeUI { get; }
        public TaskFacadeUI TaskFacadeUI { get; }
        public UserFacadeUI UserFacadeUI { get; }
        public StartFrontend()
        {
            BackendController backendController = new BackendController();
            this.BoardFacadeUI = new BoardFacadeUI(backendController);
            this.UserFacadeUI = new UserFacadeUI(backendController);
            this.TaskFacadeUI = new TaskFacadeUI(backendController);
        }
    }
}
