using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using log4net;
using System.Reflection.Metadata.Ecma335;
using IntroSE.Kanban.Backend.DataAccessLayer;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    public class UserFacade : IUserFacade
    {
        private static readonly ILog log = LogManager.GetLogger("UserFacade");
        private UserController Uc { get; }
        public Dictionary<string, User> users { get; set; }
        public IUserHistory history { get; set; }
        public bool addedUser { get; set; }
        public UserFacade(IUserHistory _uh, UserController _Uc) {
            this.users = new Dictionary<string, User>();
            this.Uc = _Uc;
            this.history = _uh;
            this.addedUser = false;
        }
        
        /// <summary>
        /// Loads the data from the data base.
        /// </summary>
        public void LoadData()
        {
            List<DTOUser> usersDto = Uc.GetAllUsers();
            // you need to convert them into 'User', of course you need make a constructor for it.
            foreach (DTOUser userDto in usersDto)
            {
                User user = new User(userDto);
                users[user.email] = user;
            }
        }
        /// <summary>
        /// Deletes the data from the data base and frim the RAM.
        /// </summary>
        public void DeleteData()
        {
            this.users.Clear();
            Uc.DeleteAllData();
        }
        /// <summary>
        /// Raturns if the password is valid
        /// </summary>
        /// <param name="password">represents the user's password</param>
        /// <returns>true if valid, else false</returns>
        public bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) //if password is null
            {
                throw new Exception("Password cannot be null");
            }

            bool existUpper = false;
            bool existSmall = false;
            bool existNumber = false;
            if (password.Length < 6 | password.Length > 20) //Length check
            {
                throw new Exception("Invalid password length");
            }
            for (int i = 0; i < password.Length; i++) //Contains capital letter, small letter, and a digit
            {
                if ((password[i] <= 'Z') & (password[i]) >= 'A')
                    existUpper = true;
                if ((password[i] <= 'z') & (password[i] >= 'a'))
                    existSmall = true;
                if ((password[i] <= '9') & (password[i] >= '0'))
                    existNumber = true;
            }
            if (!existUpper) //Missing capital letter
            {
                throw new Exception("Invalid password : it must contains a uppercase letter");
            }
            if (!existSmall) //Missing small letter
            {
                throw new Exception("Invalid password : must contains a small character");
            }
            if (!existNumber) //Missing digit
            {
                throw new Exception("Invalid password : must contains a digit");
            }
            return true;
        }
        /// <summary>
        /// Raturns if the email is valid
        /// </summary>
        /// <param name="email">represents the user's email</param>
        /// <returns>true if valid, else false</returns>
        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) // if email is null or whitespace
            {
                throw new Exception("Email cannot be null or whitespace");
            }

            if (!email.Contains("@") || !email.Contains(".")) // if email does not contain "@" or "."
            {
                throw new Exception("Invalid email format");
            }

            int atIndex = email.IndexOf("@");
            int dotIndex = email.LastIndexOf(".");

            if (atIndex > dotIndex || atIndex == dotIndex - 1) // if "@" comes after "."
            {
                throw new Exception("Invalid email format");
            }
            return true;
        }
        /// <summary>
        /// Raturns if the email is exist
        /// </summary>
        /// <param name="email">represents the user's email</param>
        /// <returns>true if exist, else false</returns>
        public bool IsEmailExists(string email)
        {
            IsValidEmail(email);
            if (!users.ContainsKey(email))
                return false;
            return true;
        }
        ///<summary>
        ///changed the email of the user
        ///</summary>
        ///<param name= "oldEmail"> The old user's email address </param>
        ///<param name = "password" > The user's password .</param>
        ///<param name = "newEmail" > The new user's email address.</param>
        public void ChangeEmail(string oldEmail, string password, string newEmail)
        {
            IsValidEmail(oldEmail); IsValidPassword(password); IsValidEmail(newEmail);
            if (users.ContainsKey(newEmail))
            {
                throw new Exception("try another email");
            }
            else if (!IsLoggedin(oldEmail))
            {
                throw new Exception("user is not logged In");
            }
            User user = GetUser(oldEmail);
            user.email = newEmail;
            users[oldEmail].ChangeEmail(oldEmail, password, newEmail);
            Uc.ChangeEmail(oldEmail,newEmail, password);
        }
        ///<summary>
        ///changed the password of the user
        ///</summary>
        ///<param name= "email"> The user's email address </param>
        ///<param name = "oldPassword" > The user's old password .</param>
        ///<param name = "newPassword" > The user's new password.</param>
        public void ChangePassword(string email, string oldPassword, string newPassword)
        {
            IsValidEmail(email); IsValidPassword(oldPassword); IsValidPassword(newPassword);
            if (!users.ContainsKey(email))
            {
                throw new Exception("email is wrong");
            }
            else if (!IsLoggedin(email))
            {
                throw new Exception("user is not logged In");
            }
            else if (users[email].password != oldPassword) {
                throw new Exception("password is incorrect");
            }
            else
            {
                User user = GetUser(email);
                user.password = newPassword;
                users[email].ChangePassword(newPassword, email, oldPassword);
                Uc.ChangePassword(email, newPassword);
            }
        }
        /// <summary>
        /// Raturns if the user is logged in
        /// </summary>
        /// <param name="email">represents the user's email</param>
        /// <returns>true if logged in, else false</returns>
        public bool IsLoggedin(string email)
        {
            IsValidEmail(email);
            if (!users.ContainsKey(email))
                throw new Exception("user doesnt exist");
            return users[email].isLoggedIn == true;
        }
        ///<summary>
        ///register new user
        ///</summary>
        ///<param name= "email"> The user's email address </param>
        ///<param name = "password" > The user's password .</param>
        public void Register(string email, string password)
        {
            IsValidEmail(email);
            IsValidPassword(password);
            if (users.ContainsKey(email))
                throw new Exception("user already exist");
            User u = new (Uc, email, password);
            users.Add(email, u);
            List<ITask> tasks = new List<ITask>();
            history.TasksInProgress.Add(email, tasks);
            this.addedUser = true;
            Uc.Register(email, password);
        }
        ///<summary>
        ///get the user
        ///</summary>
        ///<param name= "email"> The user's email address </param>
        ///<returns>the wanted user.</returns>
        public User GetUser(string email)
        {
            IsValidEmail(email);
            if (!(users.ContainsKey(email)))
                throw new Exception($"Email {email} does not exist");
            else
                return (User)users[email];
        }
        ///<summary>
        ///login the user
        ///</summary>
        ///<param name= "email"> The user's email address </param>
        ///<param name= "password"> The user's password </param>
        ///<returns>the user that logged in.</returns>
        public User Login(string email, string password)
        {
            IsValidEmail(email);
            IsValidPassword(password);
            if (!users.ContainsKey(email))
                throw new Exception("user doesnt exist");
            users[email].isLoggedIn = true;
            User user = users[email];
            if (user.password != password)
                throw new Exception("Incorrect password");
            Uc.Login(email,password);   
            return users[email];
        }
        ///<summary>
        ///logout the user
        ///</summary>
        ///<param name= "email"> The user's email address </param>
        public void Logout(string email)
        {
            IsValidEmail(email);
            if (!users.ContainsKey(email))
            {
                throw new Exception("user is not registered");
            }
            User user = (User)users[email];
            Uc.Logout(email);
            if (!user.isLoggedIn)
                throw new Exception("user is not logged in");
            user.isLoggedIn = false;
        }
    }
}
