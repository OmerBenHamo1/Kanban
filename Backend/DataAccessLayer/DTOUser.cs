using IntroSE.Kanban.Backend.BuisnessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class DTOUser
    {
        private UserController controller { get; set; }
        internal int isPersisted { get; set; } 
        internal string email { get; set; }
        internal string password { get; set; }
        internal int loggedIn { get; set; } 
        public DTOUser(UserController Uc, string email, string password, int loggedIn, int isPersisted)
        {
            this.email = email;
            this.password = password;
            this.controller = Uc;
            this.isPersisted = isPersisted ;
            this.loggedIn = loggedIn;
        }
    }
}
