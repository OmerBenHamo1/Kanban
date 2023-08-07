using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BuisnessLayer;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    [Serializable]
    public class BoardToSend
    {
        public int Id { get; set; }
        public string BoardName { get; set; }
        public string Owner { get; set; }
        public List<string> Members { get; set; }
        public List<List<TaskToSend>> Columns { get; set; }
        public int limitTaskInColumn0 { get; set;}
        public int limitTaskInColumn1 { get; set;}
        public int limitTaskInColumn2 { get; set; }
        public BoardToSend(BoardToSend board)
        {
            this.Id = board.Id;
            this.BoardName = board.BoardName;
            this.Owner = board.Owner;
            this.Members = board.Members;
            this.Columns = board.Columns;
            this.limitTaskInColumn0 = board.limitTaskInColumn0;
            this.limitTaskInColumn1 = board.limitTaskInColumn1;
            this.limitTaskInColumn2 = board.limitTaskInColumn2;
        }
        [JsonConstructor]
        public BoardToSend(int Id, string BoardName, string Owner, List<string> Members, List<List<TaskToSend>> Columns, int limitTaskInColumn0, int limitTaskInColumn1, int limitTaskInColumn2)
        {
            this.Id = Id;
            this.BoardName = BoardName;
            this.Owner = Owner;
            this.Members = Members;
            this.Columns = Columns;
            this.limitTaskInColumn0 = limitTaskInColumn0;
            this.limitTaskInColumn1 = limitTaskInColumn1;
            this.limitTaskInColumn2 = limitTaskInColumn2;
        }
        public BoardToSend(IBoard board)
        {
            this.Id = board.Id;
            this.BoardName = board.boardName;
            this.Owner = board.email;
            this.Members = board.Members;

            this.Columns = new List<List<TaskToSend>>
            {
                new List<TaskToSend>(),
                new List<TaskToSend>(),
                new List<TaskToSend>()
            };
            for(int i = 0; i < board.Columns.Count(); i++)
            {
                foreach(var item in board.Columns[i])
                {
                    this.Columns[i].Add(new TaskToSend(item));
                }
            }
            this.limitTaskInColumn0 = board.limitTaskInColumn0;
            this.limitTaskInColumn1 = board.limitTaskInColumn1;
            this.limitTaskInColumn2 = board.limitTaskInColumn2;
        }
    }
}
