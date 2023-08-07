using IntroSE.Kanban.Backend.BuisnessLayer;
using IntroSE.Kanban.Frontend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Frontend.ViewModel
{
    public class UserMainSpaceVM : NotifiableObject
    {
        private StartFrontend StartFrontend { get;}
        public UserMainSpaceVM(StartFrontend startFrontend, UserUI user) {
            User = user;
            this.StartFrontend = startFrontend;
            Boards = new List<int>();
            LoadBoardsId();
        }
        public UserUI User { get; }
        public List<int> Boards { get; }
        private string error = "";
        public string Error
        {
            get => error; set
            {
                error = value;
                RaisePropertyChanged("Error");
            }
        }
        private void LoadBoardsId()
        {
            try
            {
                List<int> temp = StartFrontend.BoardFacadeUI.GetUserBoards(User.Email);
                if (temp == null) Error = "couldn't load boards id's";
                else
                {
                    Boards.Clear();
                    Boards.AddRange(temp);
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
        }
        public List<TaskUI>? GetInProgressTasks()
        {
            try
            {
                List<TaskUI> inProgressTasks = StartFrontend.TaskFacadeUI.InProgressTasks(User.Email);
                Error = "";
                return inProgressTasks;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return null;
            }
        }

        public void Logout()
        {
            try
            {
                StartFrontend.UserFacadeUI.Logout(User.Email);
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
        }
    }
}
