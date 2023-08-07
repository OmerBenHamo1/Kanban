using IntroSE.Kanban.Backend.BuisnessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class DTOBoard
    {
        public BoardController controller { get; set; }
        internal int isPersisted { get; set; } // 0-false, 1-true
        public int Id { get; }
        public string boardName { get; }
        public string email { get; set; }
        public List<string> Members { get; set; }
        public List<List<DTOTask>> Columns { get; set; }
        public int limitTaskInColumn0 { get; set; }
        public int limitTaskInColumn1 { get; set; }
        public int limitTaskInColumn2 { get; set; }
        public DTOBoard(BoardController controller, int isPersisted,int Id, string boardName, string email, int limitTaskInColumn0, int limitTaskInColumn1, int limitTaskInColumn2)
        {
            this.controller = controller;
            this.isPersisted = isPersisted;
            this.Id = Id;
            this.email = email;
            this.boardName = boardName;
            this.limitTaskInColumn0 = limitTaskInColumn0;
            this.limitTaskInColumn1 = limitTaskInColumn1;
            this.limitTaskInColumn2 = limitTaskInColumn2;
            this.Members = new List<string>();
            this.Columns = new List<List<DTOTask>>
            {
                new List<DTOTask>(),
                new List<DTOTask>(),
                new List<DTOTask>()
            };
        }
    }
}
