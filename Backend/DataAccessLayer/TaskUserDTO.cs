using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class TaskUserDTO
    {
        internal string email { get; set; }
        internal int taskID { get; set; }
    }
}
