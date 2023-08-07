using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    public class ExceptionThrower
    {
        public ExceptionThrower() { }
        public void EmailNoExist(IUserFacade Uf, string email)
        {
            if (!Uf.IsEmailExists(email))
                throw new Exception("Email doesn't exist!");
        }
        public void UserLoggedIn(User user)
        {
            if (!user.isLoggedIn)
                throw new Exception("user is not logged in");
        }
        public void BoardNotFound(Board board)
        {
            if (board == null) 
                throw new Exception("Board doesn't exist!");
        }
        public void IsBoardOwner(Board board,string email)
        {
            if (board.email != email) 
                throw new Exception($"{email} is not the owner of board {board.boardName}");
        }
        public void IsNotBoardOwner(Board board, string email)
        {
            if (board.email == email)
                throw new Exception($"{email} is the owner of board {board.boardName}, can't leave!");
        }
        public void IsBoardExist(Dictionary<string,List<Board>> Boards,string email,string boardName)
        {
            foreach (var b in Boards[email])
            {
                if (b.boardName == boardName)
                    throw new Exception("Board already exists!");
            }
        }
        public void IsNullOrWhiteSpace(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) 
                throw new Exception("input null or whitespace");
        }
        public void LegalId(int Id, int numberOfIdTotal)
        {
            if (Id >= numberOfIdTotal | Id < 0) 
                throw new Exception($"The element with Id: {Id} doesn't exist");
        }
        public void IsAMember(Board board, string email)
        {
            if (board.Members.Contains(email)) 
                throw new Exception($"{email} is already a member of the board! Can't be joined");
        }
        public void IsNotMember(Board board, string email)
        {
            if (!board.Members.Contains(email)) 
                throw new Exception("A task can be added to a board only by its owner or members");
        }
        public void TitleLength(string title, int limit)
        {
            if (title.Length > 50) 
                throw new Exception("The title is too long!");
        }
        public void LegalColumnOrdinal(int columnOrdinal)
        {
            if (columnOrdinal > 2 | columnOrdinal < 0)
                throw new Exception("There are only 3 columns!");
        }
        public void LegalDueDate(DateTime newDueDate)
        {
            if (newDueDate < DateTime.Now)
                throw new Exception("Due Date allready passed!");
        }
    }
}
