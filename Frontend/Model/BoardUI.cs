using IntroSE.Kanban.Backend.BuisnessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;
using IntroSE.Kanban.Frontend.Model;
using System.Collections.Generic;
using System.Linq;

namespace IntroSE.Kanban.Frontend.Model
{
    public class BoardUI : NotifiableModelObject
    {
        public BoardUI(BoardToSend board, BackendController backendController) : base(backendController) {
            Id = board.Id;
            BoardName = board.BoardName;
            Owner = board.Owner;
            Members = board.Members;
            this.Columns = new List<List<TaskUI>>
            {
                new List<TaskUI>(),
                new List<TaskUI>(),
                new List<TaskUI>()
            };
            for (int i = 0; i < board.Columns.Count(); i++)
            {
                foreach (var item in board.Columns[i])
                {
                    this.Columns[i].Add(new TaskUI(item, backendController));
                }
            }
            limitTaskInColumn0 = board.limitTaskInColumn0;
            limitTaskInColumn1 = board.limitTaskInColumn1;
            limitTaskInColumn2 = board.limitTaskInColumn2;
        }
        public int Id { get; }
        public string BoardName { get; }
        public string Owner { get; }
        public List<string> Members { get; set; }
        public List<List<TaskUI>> Columns { get; }
        private int limitTaskInColumn0;
        public int LimitTaskInColumn0
        {
            get => limitTaskInColumn0; set
            {
                Controller.St.bs.LimitColumn(Owner, BoardName, 0, value);
                limitTaskInColumn0 = value;
                RaisePropertyChanged("limitTaskInColumn0");
            }
        }
        public int limitTaskInColumn1 { get; }
        public int limitTaskInColumn2 { get; }
    }
}
