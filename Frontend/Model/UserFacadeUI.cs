using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IntroSE.Kanban.Frontend.Model
{
    public class UserFacadeUI: NotifiableModelObject
    {

        public UserFacadeUI(BackendController backendController): base(backendController) { }
        public UserUI Login(string email, string password)
        {
            string Json = this.Controller.St.us.Login(email, password);
            var response = JsonSerializer.Deserialize<Response>(Json);
            if (response == null)
                throw new Exception("response is null");
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);
            UserToSend userToSend = new UserToSend(JsonSerializer.Deserialize<string>((JsonElement)response.ReturnValue));
            if (userToSend == null) throw new Exception("user to send is null");
            return new UserUI(userToSend, this.Controller);
        }
        public bool Logout(string email)
        {
            string Json = this.Controller.St.us.Logout(email);
            var response = JsonSerializer.Deserialize<Response>(Json);
            if (response == null)
                throw new Exception("response is null");
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);
            return true;
        }
        public string Register(string email, string password)
        {
            string Json = this.Controller.St.us.Register(email, password);
            var response = JsonSerializer.Deserialize<Response>(Json);
            if (response == null)
                throw new Exception("response is null");
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);
            return email + " registered successfully";
        }
        public string ChangePassword(string email, string oldPassword, string newPassword)
        {
            string Json = this.Controller.St.us.ChangePassword(email, oldPassword, newPassword);
            var response = JsonSerializer.Deserialize<Response>(Json);
            if (response == null)
                throw new Exception("response is null");
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);
            return JsonSerializer.Deserialize<string>((JsonElement)response.ReturnValue);
        }
        public string ChangeEmail(string oldEmail, string password, string newEmail)
        {
            string Json = this.Controller.St.us.ChangeEmail(oldEmail, password, newEmail);
            var response = JsonSerializer.Deserialize<Response>(Json);
            if (response == null)
                throw new Exception("response is null");
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);
            return JsonSerializer.Deserialize<string>((JsonElement)response.ReturnValue);
        }
    }
}
