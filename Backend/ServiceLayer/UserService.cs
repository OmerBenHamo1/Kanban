using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BuisnessLayer;
using System.Text.Json;
using System.Text.Json.Serialization;
using log4net;


namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class UserService : IUserService
    {
        private static readonly ILog log = LogManager.GetLogger("UserFacade");
        public IUserFacade uf { get; }
        public UserService(IUserFacade _uf)
        {
            this.uf = _uf;
        }
        ///<summary>
        ///changed the email of the user
        ///</summary>
        ///<param name= "oldEmail"> The old user's email address </param>
        ///<param name = "password" > The user's password .</param>
        ///<param name = "newEmail" > The new user's email address.</param>
        ///<returns>JSON object represents a text about the success of the func, or an error if accured.</returns>
        public string ChangeEmail(string oldEmail, string password, string newEmail)
        {
            try
            {
                log.Info($"An attempt to changed old email {oldEmail} to a new email {newEmail}.");
                uf.ChangeEmail(oldEmail, password, newEmail);
                Response response = new("The email changed succesfully", null);
                return JsonSerializer.Serialize(response);
            }
            catch (Exception e)
            {
                log.Error($"Tried to change email. old email {oldEmail} , new email: {newEmail}. Error: {e.Message}");
                Response response = new(null, e.Message);
                string json = JsonSerializer.Serialize<Response>(response);
                return json;
            }
        }
        ///<summary>
        ///changed the password of the user
        ///</summary>
        ///<param name= "email"> The user's email address </param>
        ///<param name = "oldPassword" > The user's old password .</param>
        ///<param name = "newPassword" > The user's new password.</param>
        ///<returns>JSON object represents a text about the success of the func, or an error if accured.</returns>
        public string ChangePassword(string email, string oldPassword, string newPassword)
        {
            try
            {
                log.Info($"An attempt to changed old password {oldPassword} to a new password {newPassword}.");
                uf.ChangePassword(email,oldPassword,newPassword);
                Response response = new("The password changed succesfully",null);
                return JsonSerializer.Serialize(response);
            }
            catch (Exception e)
            {
                log.Error($"Tried to change password. old password {oldPassword} , new password: {newPassword}. Error: {e.Message}"); 
                Response response = new(null, e.Message);
                string json = JsonSerializer.Serialize<Response>(response);
                return json;
            }
        }
        ///<summary>
        ///login the user
        ///</summary>
        ///<param name= "email"> The user's email address </param>
        ///<param name = "password" > The user's password .</param>
        ///<returns>JSON object represents a text about the success of the func, or an error if accured.</returns>
        public string Login(string email, string password)
        {
            try
            {
                log.Info($"An attempt to login user: {email}.");
                User success = uf.Login(email, password);
                UserToSend userToSend = new(success);
                Response response = new(userToSend.Email, null);
                return JsonSerializer.Serialize(response);
            }
            catch (Exception e)
            {
                log.Error($"Tried to login user. user's email: {email}. Error: {e.Message}");
                Response response = new(null, e.Message);
                string json = JsonSerializer.Serialize<Response>(response);
                return json;
            }
        }
        ///<summary>
        ///logout the user
        ///</summary>
        ///<param name= "email"> The user's email address </param>
        ///<returns>JSON object represents a text about the success of the func, or an error if accured.</returns>
        public string Logout(string email)
        {
            try
            {
                log.Info($"An attempt to logout user: {email}.");
                uf.Logout(email);
                Response response = new(null, null);
                return JsonSerializer.Serialize(response);
            }
            catch (Exception e)
            {
                log.Error($"Tried to logout user. user's email: {email}. Error: {e.Message}");
                Response response = new(null, e.Message);
                string json = JsonSerializer.Serialize<Response>(response);
                return json;
            }
        }
        ///<summary>
        ///register new user
        ///</summary>
        ///<param name= "email"> The user's email address </param>
        ///<param name = "password" > The user's password .</param>
        ///<returns>JSON object represents a text about the success of the func, or an error if accured.</returns>
        public string Register(string email, string password)
        {
            try
            {
                uf.Register(email, password);
                Response response = new(null, null);
                log.Info($"An attempt to register new user. email: {email}, password : {password}.");
                return JsonSerializer.Serialize(response);
            }
            catch (Exception e)
            {
                log.Error($"Tried to register new user. email: {email}, password : {password}.Error: {e.Message}");
                Response response = new(null, e.Message);
                string json = JsonSerializer.Serialize<Response>(response);
                return json;
            }
        }
    }
}
