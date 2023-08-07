using IntroSE.Kanban.Backend.ServiceLayer;
using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.BackendTests
{
    public class BoardTest
    {
        private BoardService bs { get; set; }
        private UserService us { get; set; }
        public BoardTest(BoardService _bs, UserService _us)
        {
            us = _us;
            bs = _bs;
        }
        public void TestsCreateBoard()
        {
            //attempt to create new Board with title 'testBoard' - should succeed
            //assuming that there is no Board exist with this title
            string json1 = bs.CreateBoard("liransemail@gmail.com", "liran's board");
            var response1 = JsonSerializer.Deserialize<Response>(json1);
            if (response1 == null)
                Console.WriteLine("Response is null");
            else if (response1.ReturnValue == null)
                Console.WriteLine(response1.ErrorMessage);
            else
            {
                Console.WriteLine("Board was created successfully!");
            }

            //attempt to create a new Board with a title that already exists
            //in the kanban - should return an Error.
            //assuming that Board with this title already exist.
            string json2 = bs.CreateBoard("liransemail@gmail.com", "liran's board");
            var response2 = JsonSerializer.Deserialize<Response>(json2);
            if (response2 == null)
                Console.WriteLine("response is null!"); 
            else
            {
                if (response2.ReturnValue == null)
                    Console.WriteLine("yes! new Board already exist!!!");
                else
                {
                    Console.WriteLine("Wasn't suppose to add new Board, already exists :(");
                }
            }
            //attempt to add new Board with an empty title - should return Error
            string json3 = bs.CreateBoard("liransemail@gmail.com", "");
            var response3 = JsonSerializer.Deserialize<Response>(json3);
            if (response3 == null)
                Console.WriteLine("response is null!");
            else
            {
                if (response3.ReturnValue == null)
                    Console.WriteLine("yes! board with empty title failed!!!");
                else
                {
                    Console.WriteLine("Wasn't suppose to add Board with empty title :(");
                }
            }
        }

        public void TestsDeleteBoard()
        {
            // attemp to delete a board - should succeed.
            // assume that a Board with the input below exist.
            string json10 = bs.DeleteBoard("liransemail@gmail.com", "liran's board");
            var response10 = JsonSerializer.Deserialize<Response>(json10);
            if (response10 == null)
                Console.WriteLine("response is null!");
            else
            {
                if (response10.ReturnValue == null)
                    Console.WriteLine(response10.ErrorMessage);
                else
                {
                    Console.WriteLine("Board was deleted successfully!");
                }
            }
            // attemp to delete a board - should return an error.
            // assume that a Board with the input below  doesn't exist.
            string json11 = bs.DeleteBoard("liransemail@gmail.com", "liran's board");
            var response11 = JsonSerializer.Deserialize<Response>(json11);
            if (response11 == null)
                Console.WriteLine("response is null!");
            else
            {
                if (response11.ReturnValue == null)
                    Console.WriteLine("yes! it didn't delete a Board that doesn't exist!!!");
                else
                {
                    Console.WriteLine("Wasn't suppose to delete Board, doesn't exist :(");
                }
            }
        }

        public void TestsLimitNumTask()
        {
            // attemp to limit a column - should succeed.
            // assume that the data exist in the system.
            string json20 = bs.LimitColumn("liransemail@gmail.com", "liran's board", 1, 5);
            var response20 = JsonSerializer.Deserialize<Response>(json20);
            if (response20 == null)
                Console.WriteLine("response is null!");
            else
            {
                if (response20.ReturnValue == null)
                    Console.WriteLine(response20.ErrorMessage);
                else
                {
                    Console.WriteLine("Column was limited successfully!");
                }
            }
            // attemp to limit a column in non - existing column - should return an error.
            string json21 = bs.LimitColumn("liransemail@gmail.com", "liran's board", 5, 5);
            var response21 = JsonSerializer.Deserialize<Response>(json21);
            if (response21 == null)
                Console.WriteLine("response is null!");
            else
            {
                if (response21.ReturnValue == null)
                    Console.WriteLine("yes! it didn't limit in non-existing column!!!");
                else
                {
                    Console.WriteLine("Wasn't suppose to work with non-existing column :(");
                }
            }
            // attemp to limit a column with invalid limit - should return an error.
            //assuming the limit must be bigger than 1
            string json22 = bs.LimitColumn("liransemail@gmail.com", "liran's board", 1, 1);
            var response22 = JsonSerializer.Deserialize<Response>(json22);
            if (response22 == null)
                Console.WriteLine("response is null!");
            else
            {
                if (response22.ReturnValue == null)
                    Console.WriteLine("yes! it failed - invalid limit = 1!!!");
                else
                {
                    Console.WriteLine("Wasn't suppose to work - limit = 1 :(");
                }
            }
            // attemp to limit a column with invalid limit - should return an error.
            //assuming the limit must be bigger than 0
            string json23 = bs.LimitColumn("liransemail@gmail.com", "liran's board", 1, -1);
            var response23 = JsonSerializer.Deserialize<Response>(json23);
            if (response23 == null)
                Console.WriteLine("response is null!");
            else
            {
                if (response23.ReturnValue == null)
                    Console.WriteLine("yes! it failed - limit = -1!!!");
                else
                {
                    Console.WriteLine("Wasn't suppose to work - limit = -1 :(");
                }
            }
            //attempt to limit a column to 1 tasks - should return Error
            //assuming that the column contains more tasks than 1 Tasks.
            string json24 = bs.LimitColumn("liransemail@gmail.com", "liran's board", 1, 1);
            var response24 = JsonSerializer.Deserialize<Response>(json24);
            if (response24 == null)
                Console.WriteLine("response is null!");
            else
            {
                if (response24.ReturnValue == null)
                    Console.WriteLine("yes! it failed - more tasks than the limit!!!");
                else
                {
                    Console.WriteLine("Wasn't suppose to work - more tasks than the limit :(");
                }
            }
        }

        public void TestsGetColumnLimit()
        {
            //attempt to recieve the column's limit of a column  - should succeed.
            //assunming that the limit of the column is 5.
            string json30 = bs.GetColumnLimit("liransemail@gmail.com", "liran's board", 1);
            var response30 = JsonSerializer.Deserialize<Response>(json30);
            if (response30 == null)
                Console.WriteLine("response is null!");
            else
            {
                if (response30.ReturnValue == null)
                    Console.WriteLine(response30.ErrorMessage);
                else
                {
                    Console.WriteLine("Column limit was extracted successfully!");
                }
            }
        }
        public void TestsGetColumnName()
        {
            //attempt to recieve the column's name of a column  - should succeed.
            // assuming that the name of the column is "InProgress".
            string json40 = bs.GetColumnName("liransemail@gmail.com", "liran's board", 1);
            var response40 = JsonSerializer.Deserialize<Response>(json40);
            if (response40 == null)
                Console.WriteLine("response is null!");
            else
            {
                if (response40.ReturnValue == null)
                    Console.WriteLine(response40.ErrorMessage);
                else
                {
                    Console.WriteLine("Column name was extracted successfully!");
                }
            }
        }
        // an attempt to get a list with all the user's boards, should succeed
        public void TestsGetUserBoards()
        {
            us.Register("omer24@gmail.com", "123456Ab");
            bs.CreateBoard("board1", "omer24@gmail.com");
            bs.CreateBoard("board2", "omer24@gmail.com");
            bs.CreateBoard("board3", "omer24@gmail.com");
            String json90 = bs.GetUserBoards("omer24@gmail.com");
            var response90 = JsonSerializer.Deserialize<Response>(json90);
            if (response90 == null)
                Console.WriteLine("response is null!");
            else
            {
                if (response90.ErrorMessage.Equals(null))
                    Console.WriteLine(
                       "Account with email: omer24@gmail.com has boards 1,2,3, test successfull");
                else
                    Console.WriteLine(response90.ErrorMessage);
            }
        }
        // an attempt to leave the board, should succeed
        public void TestsLeaveBoard() 
         {
             us.Register("omer24@gmail.com", "123456Ab");
             us.Register("liran3@gmail.com", "123456Ab");
             bs.CreateBoard("board7", "omer24@gmail.com");
             bs.JoinBoard("liran3@gmail.com", 3);

            String json50 = bs.LeaveBoard("liran3@gmail.com", 3);
            var response50 = JsonSerializer.Deserialize<Response>(json50);
            if (response50 == null)
                Console.WriteLine("response is null!");
            else
            {
                if (response50.ErrorMessage.Equals(null))
                    Console.WriteLine(
                       "Account with email: liran3@gmail.com left board successfully");
                else
                    Console.WriteLine(response50.ErrorMessage);
            }
            // an attempt to leave board with user that doesnt join it - should fail.
            String json51 = bs.LeaveBoard("omer24@gmail.com", 3);
            var response51 = JsonSerializer.Deserialize<Response>(json51);
            if (response51 == null)
                Console.WriteLine("response is null!");
            else
            {
                if (response51.ErrorMessage.Equals(null))
                    Console.WriteLine(
                       "Account with email: liran3@gmail.com left board successfully. \n wasn't suppose to succeed");
                else
                    Console.WriteLine("supposed to failed. Good!");
            }
        }
        // an attempt to join board, should succeed

        public void TestsJoinBoard() 
        {
            us.Register("omer25@gmail.com", "123456Ab");
            us.Register("liran3@gmail.com", "123456Ab");
            bs.CreateBoard("board8", "omer25@gmail.com");

            String json60 = bs.JoinBoard("liran3@gmail.com", 8);
            var response60 = JsonSerializer.Deserialize<Response>(json60);
            if (response60 == null)
                Console.WriteLine("response is null!");
            else
            {
                if (response60.ErrorMessage.Equals(null))
                    Console.WriteLine(
                       "Account with email: liran3@gmail.com join board successfully");
                else
                    Console.WriteLine(response60.ErrorMessage);
            }
            // an attempt to join a board with user that dont exist, should fail.
            String json61 = bs.JoinBoard("idan77@gmail.com", 8);
            var response61 = JsonSerializer.Deserialize<Response>(json61);
            if (response61 == null)
                Console.WriteLine("response is null!");
            else
            {
                if (response61.ErrorMessage.Equals(null))
                    Console.WriteLine(
                       "Account with email: liran3@gmail.com join board successfully. \n wasn't supposed to succeed");
                else
                    Console.WriteLine("supposed to failed. Good!");
            }
        }
        //an attempt to get the board name by his id number, should cucceed
        public void TestsGetBoardName()
        {
            us.Register("omer88@gmail.com", "123456Ab");
            bs.CreateBoard("board88", "omer88@gmail.com");

            String json70 = bs.GetBoardName(88);
            var response70 = JsonSerializer.Deserialize<Response>(json70);
            if (response70 == null)
                Console.WriteLine("response is null!");
            else
            {
                if (response70.ErrorMessage.Equals(null))
                    Console.WriteLine(
                        "Account with email:omer88@gmail.com ,  board name with id = 88 retrieved successfully successfully");
                else
                    Console.WriteLine(response70.ErrorMessage);
            }
            // an attempt to get the board name with wrong id number, should fail
            String json71 = bs.GetBoardName(25);
            var response71 = JsonSerializer.Deserialize<Response>(json71);
            if (response71 == null)
                Console.WriteLine("response is null!");
            else
            {
                if (response71.ErrorMessage.Equals(null))
                    Console.WriteLine(
                        "Account with email:omer88@gmail.com ,  board name with id = 88 retrieved successfully.\n wasn't supposed to succeed");
                else
                    Console.WriteLine("supposed to failed. Good!");
            }
        }   
    }
}
