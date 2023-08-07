using IntroSE.Kanban.Backend.DataAccessLayer;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    public class User : IUser
    {
        private static readonly ILog log = LogManager.GetLogger("User");
        public DTOUser DTOUser { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public bool isLoggedIn { get; set; }
        public User(UserController Uc, string email, string password)
        {
            this.DTOUser = new DTOUser(Uc, email, password,1,1);
            this.email = email;
            this.password = password;
            this.isLoggedIn = true;
        }
        public User(DTOUser du)
        {
            this.email = du.email;
            this.password = du.password;
            this.isLoggedIn = true;
            du.loggedIn = 1;
        }
        public void ChangePassword(string newPassword, string email, string OldPassword)
        {
            this.password = newPassword;
        }
        public void ChangeEmail(string oldEmail, string password, string newEmail)
        {
            this.email = newEmail;
        }
    }
}

