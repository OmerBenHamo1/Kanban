using IntroSE.Kanban.Backend.BuisnessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class UserToSend
    {
        public string Email;
        public UserToSend(IUser user)
        {
            Email = user.email;
        }
        public UserToSend(string email)
        {
            Email = email;
        }
    }
}
