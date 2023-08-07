using IntroSE.Kanban.Frontend.ViewModel;

namespace IntroSE.Kanban.Frontend.Model
{
    public class NotifiableModelObject : NotifiableObject
    {
        public BackendController Controller { get; set; }
        protected NotifiableModelObject(BackendController controller)
        {
            this.Controller = controller;
        }
    }
}
