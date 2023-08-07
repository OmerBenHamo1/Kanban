using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    public interface IUserFacade
    {
        Dictionary<string, User> users { get; set; }
        bool IsValidPassword(string password);
        bool IsValidEmail(string email);
        User GetUser(string email);
        bool addedUser { get; set; }
        void LoadData();
        void DeleteData();
        void ChangeEmail(string oldEmail, string password, string newEmail);
        bool IsEmailExists(string email);
        void ChangePassword(string email, string oldPassword, string newPassword);
        void Register(string email, string password);
        User Login(string email, string password);
        void Logout(string email);
    }
}
