using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.Frontend.Model
{
    public class BackendController
    {
        public StartService St { get; set; }
        public BackendController()
        {
            this.St = new StartService(true);
        }
    }
}
