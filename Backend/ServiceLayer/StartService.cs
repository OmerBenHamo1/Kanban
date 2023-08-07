using IntroSE.Kanban.Backend.BuisnessLayer;
using log4net.Config;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class StartService
    {
        public UserService us ;
        public BoardService bs;
        public TaskService ts;
        public StartService() {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            UserController Uc = new (path);
            TaskController Tc = new (path,Uc);
            BoardController Bc = new (path,Uc,Tc);
            UserHistory uh = new();
            UserFacade uf = new(uh,Uc);
            BoardFacade bf = new (uf,uh, Bc);
            us = new(uf);
            ts = new(bf);
            bs = new(bf,uf);
        }
        public StartService(bool load)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            UserController Uc = new(path);
            TaskController Tc = new(path, Uc);
            BoardController Bc = new(path, Uc, Tc);
            UserHistory uh = new();
            UserFacade uf = new(uh, Uc);
            BoardFacade bf = new(uf, uh, Bc);
            us = new(uf);
            ts = new(bf);
            bs = new(bf, uf);
            bs.LoadData();
           
        }
        public UserService GetUserService { get { return us; } }
        public BoardService GetBoardService{ get { return bs; } }
        public TaskService GetTaskService { get { return ts; } }
    }
}
