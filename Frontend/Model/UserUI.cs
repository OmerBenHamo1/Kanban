using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IntroSE.Kanban.Frontend.Model
{
    public class UserUI: NotifiableModelObject
    {
        public UserUI(UserToSend user, BackendController backendController) : base(backendController) 
        { 
            this.Email = user.Email;
        }
        public string Email { get; set; }
    }
}
