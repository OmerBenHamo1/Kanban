using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using IntroSE.Kanban.Frontend;
using IntroSE.Kanban.Frontend.Model;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.Frontend.Model
{
    public class BoardFacadeUI: NotifiableModelObject
    {
        public BoardFacadeUI(BackendController backendController): base(backendController) { }
        public BoardUI CreateBoard(string email, string boardName)
        {
            string Json = this.Controller.St.bs.CreateBoard(email, boardName);
            var response = JsonSerializer.Deserialize<Response>(Json);
            if (response == null)
                throw new Exception("response is null");
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);
            var boardToSend = JsonSerializer.Deserialize<BoardToSend>((JsonElement)response.ReturnValue);
            return new BoardUI(boardToSend, this.Controller);
        }
        public string DeleteBoard(string email, string boardName)
        {
            string Json = this.Controller.St.bs.DeleteBoard(email, boardName);
            var response = JsonSerializer.Deserialize<Response>(Json);
            if (response == null)
                throw new Exception("response is null");
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);
            return JsonSerializer.Deserialize<string>( response.ReturnValue.ToString());
        }
        public BoardUI GetBoard(string email, string boardName)
        {
            string Json = this.Controller.St.bs.GetBoard(email, boardName);
            var response = JsonSerializer.Deserialize<Response>(Json);
            if (response == null)
                throw new Exception("response is null");
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);
            var boardToSend = JsonSerializer.Deserialize<BoardToSend>((JsonElement)response.ReturnValue);
            return new BoardUI(boardToSend, this.Controller);
        }
        public BoardUI GetBoard(int boardID)
        {
            string Json = this.Controller.St.bs.GetBoard(boardID);
            var response = JsonSerializer.Deserialize<Response>(Json);
            if (response == null)
                throw new Exception("response is null");
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);
            var boardToSend = JsonSerializer.Deserialize<BoardToSend>((JsonElement) response.ReturnValue);
            return new BoardUI(boardToSend, this.Controller);
        }
        public int LimitColumn(string email, string boardName, int columnOrdinal, int limit)
        {
            string Json = this.Controller.St.bs.LimitColumn(email, boardName, columnOrdinal, limit);
            var response = JsonSerializer.Deserialize<Response>(Json);
            if (response == null)
                throw new Exception("response is null");
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);
            return JsonSerializer.Deserialize<int>((JsonElement)response.ReturnValue);
        }
        public int GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            string Json = this.Controller.St.bs.GetColumnLimit(email, boardName, columnOrdinal);
            var response = JsonSerializer.Deserialize<Response>(Json);
            if (response == null)
                throw new Exception("response is null");
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);
            return JsonSerializer.Deserialize<int>((JsonElement)response.ReturnValue);
        }
        public string GetColumnName(string email, string boardName, int columnOrdinal)
        {
            string Json = this.Controller.St.bs.GetColumnName(email, boardName, columnOrdinal);
            var response = JsonSerializer.Deserialize<Response>(Json);
            if (response == null)
                throw new Exception("response is null");
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);
            return JsonSerializer.Deserialize<string>((JsonElement)response.ReturnValue);
        }
        public List<TaskUI> GetColumn(string email, string boardName, int columnOrdinal)
        {
            string Json = this.Controller.St.bs.GetColumn(email, boardName, columnOrdinal);
            var response = JsonSerializer.Deserialize<Response>(Json);
            if (response == null)
                throw new Exception("response is null");
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);
            List<TaskUI> column = new List<TaskUI>();
            List<TaskToSend> taskToSends = JsonSerializer.Deserialize<List<TaskToSend>>((JsonElement)response.ReturnValue);
            foreach(var t in taskToSends)
            {
                column.Add(new TaskUI(t, Controller));
            }
            return column;
        }
        public List<int> GetUserBoards(string email)
        {
            string Json = this.Controller.St.bs.GetUserBoards(email);
            var response = JsonSerializer.Deserialize<Response>(Json);
            if (response == null)
                throw new Exception("response is null");
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);
            return JsonSerializer.Deserialize<List<int>>( (JsonElement)response.ReturnValue);
        }
        public string JoinBoard(string email, int boardID)
        {
            string Json = this.Controller.St.bs.JoinBoard(email, boardID);
            var response = JsonSerializer.Deserialize<Response>(Json);
            if (response == null)
                throw new Exception("response is null");
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);
            return email + " joined Board " + boardID + " successfully!";
        }
        public string LeaveBoard(string email, int boardID)
        {
            string Json = this.Controller.St.bs.LeaveBoard(email, boardID);
            var response = JsonSerializer.Deserialize<Response>(Json);
            if (response == null)
                throw new Exception("response is null");
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);
            return email + " left Board " + boardID + " successfully!";
        }
        public string GetBoardName(int boardID)
        {
            string Json = this.Controller.St.bs.GetBoardName(boardID);
            var response = JsonSerializer.Deserialize<Response>(Json);
            if (response == null)
                throw new Exception("response is null");
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);
            return JsonSerializer.Deserialize<string>( (JsonElement)response.ReturnValue);
        }
    }
}
