using IntroSE.Kanban.Backend.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    public interface IUser
    {
        string email { get; set; }
        string password { get; set; }
        void ChangePassword(string oldPassword, string email, string newPassword );
        void ChangeEmail(string oldEmail, string password, string newEmail);
        public DTOUser DTOUser { get; set; }
    }
}
