using System;
using System.Collections.Generic;
using TenmoClient.Models;

namespace TenmoClient
{
    public class ConsoleService
    {
        /// <summary>
        /// Prompts for transfer ID to view, approve, or reject
        /// </summary>
        /// <param name="action">String to print in prompt. Expected values are "Approve" or "Reject" or "View"</param>
        /// <returns>ID of transfers to view, approve, or reject</returns>
        public int PromptForTransferID(string action)
        {
            Console.WriteLine("");
            Console.Write("Please enter transfer ID to " + action + " (0 to cancel): ");
            if (!int.TryParse(Console.ReadLine(), out int auctionId))
            {
                Console.WriteLine("Invalid input. Only input a number.");
                return 0;
            }
            else
            {
                return auctionId;
            }
        }

        public LoginUser PromptForLogin()
        {
            Console.Write("Username: ");
            string username = Console.ReadLine();
            string password = GetPasswordFromConsole("Password: ");

            LoginUser loginUser = new LoginUser
            {
                Username = username,
                Password = password
            };
            return loginUser;
        }

        private string GetPasswordFromConsole(string displayMessage)
        {
            string pass = "";
            Console.Write(displayMessage);
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                // Backspace Should Not Work
                if (!char.IsControl(key.KeyChar))
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass = pass.Remove(pass.Length - 1);
                        Console.Write("\b \b");
                    }
                }
            }
            // Stops Receving Keys Once Enter is Pressed
            while (key.Key != ConsoleKey.Enter);
            Console.WriteLine("");
            return pass;
        }

        public void PrintBalance(decimal? balance)
        {
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("Balance Details");
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine(" Your current account balance is: $" + balance);          
        }

        public void PrintAccount(Account account)
        {
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("Account Details");
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine(" Account Id: " + account.AccountId);
            Console.WriteLine(" User Id: " + account.UserId);
            Console.WriteLine(" Balance: " + account.Balance);
        }
        public void PrintUsers(List<User> users)
        {
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("Users");
            Console.WriteLine("ID          Name");
            Console.WriteLine("--------------------------------------------");
            foreach(User user in users)
            {
                Console.WriteLine($"{user.UserId}          {user.Username}");
            }
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine();
        }

        public void PrintTransfers(List<Transfer> transfers)
        {
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("Transfers");
            Console.WriteLine("ID             From/To           Amount");
            Console.WriteLine("--------------------------------------------");
            foreach (Transfer transfer in transfers)
            {
                if(transfer.Sender == 0)
                {
                    Console.WriteLine($"{transfer.TransferId}          From: {transfer.AccountFrom}            ${transfer.Amount}");
                }
                else if(transfer.Sender == 1)
                {
                    Console.WriteLine($"{transfer.TransferId}          To: {transfer.AccountTo}            ${transfer.Amount}");
                }
            }
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine();
        }

        public void PrintTransferById(Transfer transfer)
        {
            ApiService apiService = new ApiService();
            string userFrom = apiService.GetUsername(transfer.AccountFrom);
            string userTo = apiService.GetUsername(transfer.AccountTo);
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("Transfer Details");
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine(" Transfer Id: " + transfer.TransferId);
            Console.WriteLine(" From: " + userFrom);
            Console.WriteLine(" To: " + userTo);
            Console.WriteLine(" Transfer Type: " + transfer.TransferTypeId);
            Console.WriteLine(" Transfer Status: " + transfer.TransferStatusId);
            Console.WriteLine(" Amount: " + transfer.Amount);
            Console.WriteLine("--------------------------------------------");
        }
        public int PromptForUserId()
        {
            Console.WriteLine("Enter ID of user you are sending to (0 to cancel): "); //need to trap 0 value here
            return int.Parse(Console.ReadLine());  //need tryparse here
        }

        public decimal PromptForAmount()
        {
            Console.WriteLine("Enter amount: ");
            return decimal.Parse(Console.ReadLine());  //need tryparse here
        }

        public int PromptForTransferId()
        {
            Console.WriteLine("Enter ID of transfer you'd like more details of (0 to cancel): "); //need to trap 0 value here
            return int.Parse(Console.ReadLine());  //need tryparse here
        }
    }
}
