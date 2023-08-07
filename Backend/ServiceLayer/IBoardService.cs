using IntroSE.Kanban.Backend.BuisnessLayer;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public interface IBoardService
    {
        IUserFacade uf { get; set; }
        IBoardFacade bf { get; set; }
        string CreateBoard(string email, string boardName);
        string DeleteBoard(string email, string boardName);
        string GetBoard(string email, string boardName);
        string LimitColumn(string email, string boardName, int columnOrdinal, int limit);
        string GetColumnLimit(string email, string boardName, int columnOrdinal);
        string GetColumnName(string email, string boardName, int columnOrdinal);
        // new
        string LoadData();
        string DeleteData();
        string GetColumn(string email, string boardName, int columnOrdinal);
        string GetUserBoards(string email);
        string JoinBoard(string email, int boardID);
        string LeaveBoard(string email, int boardID);
        string GetBoardName(int boardId);
    }
}