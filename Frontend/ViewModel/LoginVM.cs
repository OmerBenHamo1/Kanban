
using IntroSE.Kanban.Frontend.Model;
using System;

namespace IntroSE.Kanban.Frontend.ViewModel
{
    public class LoginVM : NotifiableObject
    {
        private StartFrontend StartFrontend { get; set; }
        public LoginVM(StartFrontend startFrontend)
        {
            this.StartFrontend = startFrontend;
        }
        private string errorMessage = "";
        public string ErrorMessage
        {
            get => errorMessage; set
            {
                errorMessage = value;
                RaisePropertyChanged("ErrorMessage");
            }
        }
        private string email = "";
        public string Email
        {
            get => email; set
            {
                this.email = value;
                FieldsAreNotEmpty = string.IsNullOrWhiteSpace(value);
            }
        }
        public bool FieldsAreNotEmpty
        {
            set
            {
                RaisePropertyChanged("FieldsAreNotEmpty");
            }
            get =>
                !string.IsNullOrWhiteSpace(this.Email) && !string.IsNullOrWhiteSpace(this.Password);
        }
        private string password = "";
        public string Password
        {
            get => password; set
            {
                password = value;
                FieldsAreNotEmpty = string.IsNullOrWhiteSpace(value);
            }
        }
        public UserUI? Login()
        {
            try
            {
                var user = StartFrontend.UserFacadeUI.Login(Email, Password);
                ErrorMessage = "";
                return user;
            }
            catch(Exception ex)
            {
                ErrorMessage = ex.Message;
                return null;
            }
        }
        public void Register()
        {
            try
            {
                StartFrontend.UserFacadeUI.Register(Email, Password);
                ErrorMessage = "";
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}
