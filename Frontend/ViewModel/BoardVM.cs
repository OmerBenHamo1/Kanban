using IntroSE.Kanban.Frontend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Frontend.ViewModel
{
    public class BoardVM : NotifiableObject
    {
        private StartFrontend StartFrontend { get; }
        public BoardVM(StartFrontend StartFrontend, int boardId)
        {
            this.StartFrontend = StartFrontend;
            try
            {
                this.BoardUI = StartFrontend.BoardFacadeUI.GetBoard(boardId);
                this.Backlog = new List<string>();
                foreach (var t in this.BoardUI.Columns[0])
                {
                    this.Backlog.Add(t.Title);
                }
                this.InProgress = new List<string>();
                foreach (var t in this.BoardUI.Columns[1])
                {
                    this.InProgress.Add(t.Title);
                }
                this.Done = new List<string>();
                foreach (var t in this.BoardUI.Columns[2])
                {
                    this.Done.Add(t.Title);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
        private string errorMessage = "";
        public string ErrorMessage
        {
            get => errorMessage; set
            {
                errorMessage = value;
                RaisePropertyChanged("ErrorMessage");
            }
        }
        private BoardUI? boardUI;
        public BoardUI? BoardUI
        {
            get { return boardUI; }
            set
            {
                boardUI = value;
                RaisePropertyChanged("BoardUI");
            }
        }
        private List<string>? backlog;
        public List<string>? Backlog
        {
            get { return backlog; }
            set
            {
                backlog = value;
                RaisePropertyChanged("Backlog");
            }
        }
        private List<string>? done;
        public List<string>? Done
        {
            get { return done; }
            set
            {
                done = value;
                RaisePropertyChanged("Done");
            }
        }
        public List<string>? InProgress { get; }
    }
}
