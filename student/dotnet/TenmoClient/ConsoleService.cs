using System;
using System.Collections.Generic;
using TenmoClient.Models;

namespace TenmoClient
{
    public class ConsoleService
    {
        ApiService apiService;
        public ConsoleService(ApiService apiService)
        {
            this.apiService = apiService;
        }
        /// <summary>
        /// Prompts for transfer ID to view, approve, or reject
        /// </summary>
        /// <param name="action">String to print in prompt. Expected values are "Approve" or "Reject" or "View"</param>
        /// <returns>ID of transfers to view, approve, or reject</returns>
        public int PromptForTransferID(string action)
        {
            Program.dyn.Add("");
            Console.Write("Please enter transfer ID to " + action + " (0 to cancel): ");
            if (!int.TryParse(Console.ReadLine(), out int auctionId))
            {
                Program.dyn.Add("Invalid input. Only input a number.");
                return 0;
            }
            else
            {
                return auctionId;
            }
        }

        public LoginUser PromptForLogin()
        {
            Program.dyn.Add("Username: ");
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
            Program.dyn.Add(displayMessage);
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
            Program.dyn.Add("");
            return pass;
        }

        public void PrintBalance(decimal? balance)
        {
            Program.dyn.Add("--------------------------------------------");
            Program.dyn.Add("Balance Details");
            Program.dyn.Add("--------------------------------------------");
            Program.dyn.Add(" Your current account balance is: $" + balance);          
        }

        public void PrintAccount(Account account)
        {
            Program.dyn.Add("--------------------------------------------");
            Program.dyn.Add("Account Details");
            Program.dyn.Add("--------------------------------------------");
            Program.dyn.Add(" Account Id: " + account.AccountId);
            Program.dyn.Add(" User Id: " + account.UserId);
            Program.dyn.Add(" Balance: $" + account.Balance);
        }
        public void PrintUsers(List<User> users)
        {
            Program.dyn.Add("--------------------------------------------");
            Program.dyn.Add("Users");
            Program.dyn.Add("ID          Name");
            Program.dyn.Add("--------------------------------------------");
            foreach(User user in users)
            {
                Program.dyn.Add($"{user.UserId}          {user.Username}");
            }
            Program.dyn.Add("--------------------------------------------");
            Program.dyn.Add();
        }

        public void PrintTransfers(List<Transfer> transfers)
        {
            Program.dyn.Add("--------------------------------------------");
            Program.dyn.Add("Transfers");
            Program.dyn.Add("ID             From/To           Amount");
            Program.dyn.Add("--------------------------------------------");
            foreach (Transfer transfer in transfers)
            {
                if(transfer.Sender == 0)
                {
                    Program.dyn.Add($"{transfer.TransferId}          From: {transfer.fromUser}            ${transfer.Amount}");
                }
                else if(transfer.Sender == 1)
                {
                    Program.dyn.Add($"{transfer.TransferId}          To: {transfer.toUser}            ${transfer.Amount}");
                }
            }
            Program.dyn.Add("--------------------------------------------");
            Program.dyn.Add();
        }

        public void PrintTransferById(Transfer transfer)
        {
            string userFrom = transfer.fromUser;
            string userTo = transfer.toUser;

            string typeWord = "Send";

            string statusWord = "Approved";

            Program.dyn.Add("--------------------------------------------");
            Program.dyn.Add("Transfer Details");
            Program.dyn.Add("--------------------------------------------");
            Program.dyn.Add(" Transfer Id: " + transfer.TransferId);
            Program.dyn.Add(" From: " + userFrom);
            Program.dyn.Add(" To: " + userTo);
            Program.dyn.Add(" Transfer Type: " + typeWord);
            Program.dyn.Add(" Transfer Status: " + statusWord);
            Program.dyn.Add(" Amount: $" + transfer.Amount);
            Program.dyn.Add("--------------------------------------------");
        }
        public int PromptForUserId()
        {
            int menuSelection;
            start:
            Program.dyn.Add("Enter ID of user you are sending to (0 to cancel): ");
            
            if (!int.TryParse(Console.ReadLine(), out menuSelection))
            {
                Program.dyn.Add("Invalid input. Please enter only a number.");
                goto start;
            }
            if(menuSelection == UserService.GetUserId())
            {
                Program.dyn.Add("Cannot send money to yourself. Please enter another user's ID.");
                goto start;
            }
            return menuSelection;
        }

        public decimal PromptForAmount()
        {
            decimal menuSelection;
            start:
            Program.dyn.Add("Enter amount (0 to cancel): ");
            if (!decimal.TryParse(Console.ReadLine(), out menuSelection))
            {
                Program.dyn.Add("Invalid input. Please enter only a number.");
                goto start;
            }
            if (menuSelection < 0.00M)
            {
                Program.dyn.Add("Invalid input. Please enter a number greater than 0.");
                goto start;
            }
            if(menuSelection > apiService.GetBalance())
            {
                Program.dyn.Add("Insufficient funds. Please enter an amount within your current balance.");
                goto start;
            }
            return menuSelection;
        }

        public int PromptForTransferId()
        {
            int menuSelection;
            Program.dyn.Add("Enter ID of transfer you'd like more details of (0 to cancel): "); 
            if (!int.TryParse(Console.ReadLine(), out menuSelection))
            {
                Program.dyn.Add("Invalid input. Please enter only a number.");
            }
            return menuSelection;
        }
    }
}
