using IntroSE.Kanban.Backend.BuisnessLayer;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public interface IUserService
    {
        IUserFacade uf { get;}

        string ChangePassword(string email, string oldPassword, string newPassword);
        string ChangeEmail(string oldEmail, string password, string newEmail);
        string Register(string email, string password);
        string Login(string email, string password);
        string Logout(string email);
    }
}