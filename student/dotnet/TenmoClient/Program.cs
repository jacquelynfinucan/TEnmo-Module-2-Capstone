using System;
using System.Collections.Generic;
using TenmoClient.Models;
using RestSharp.Authenticators;

namespace TenmoClient
{
    class Program
    {
        private static readonly AuthService authService = new AuthService();
        private static readonly ApiService apiService = new ApiService("https://localhost:44315/",authService.getClient);
        private static readonly ConsoleService consoleService = new ConsoleService(apiService);
        public static DynamicConsole dyn = new DynamicConsole(); 

        static void Main(string[] args)
        {
            Run();
        }

        private static void Run()
        {
            while(true)
            {
                int loginRegister = -1;
                while (loginRegister != 1 && loginRegister != 2)
                {
                    dyn.Add("Welcome to TEnmo!");
                    dyn.Add("1: Login");
                    dyn.Add("2: Register");
                    Console.Write("Please choose an option: ");

                    if (!int.TryParse(Console.ReadLine(), out loginRegister))
                    {
                        dyn.Add("Invalid input. Please enter only a number.");
                    }
                    else if (loginRegister == 1)
                    {
                        while (!UserService.IsLoggedIn()) //will keep looping until user is logged in
                        {
                            LoginUser loginUser = consoleService.PromptForLogin();
                            ApiUser user = authService.Login(loginUser);
                            if (user != null)
                            {
                                UserService.SetLogin(user);
                                //apiService.client.Authenticator = new JwtAuthenticator(user.Token);
                            }
                        }
                    }
                    else if (loginRegister == 2)
                    {
                        bool isRegistered = false;
                        while (!isRegistered) //will keep looping until user is registered
                        {
                            LoginUser registerUser = consoleService.PromptForLogin();
                            isRegistered = authService.Register(registerUser);
                            if (isRegistered)
                            {
                                dyn.Add("");
                                dyn.Add("Registration successful. You can now log in.");
                                loginRegister = -1; //reset outer loop to allow choice for login
                            }
                        }
                    }
                    else
                    {
                        dyn.Add("Invalid selection.");
                    }
                }

                MenuSelection();
            }
        }

        private static void MenuSelection()
        {
            int menuSelection = -1;
            while (menuSelection != 0)
            {
                dyn.Add("");
                dyn.Add("Welcome to TEnmo! Please make a selection: ");
                dyn.Add("1: View your current balance");
                dyn.Add("2: View your past transfers");
                dyn.Add("3: View your pending requests");
                dyn.Add("4: Send TE bucks");
                dyn.Add("5: Request TE bucks");
                dyn.Add("6: Log in as different user");
                dyn.Add("0: Exit");
                dyn.Add("---------");
                Console.Write("Please choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out menuSelection))
                {
                    dyn.Add("Invalid input. Please enter only a number.");
                }
                else if (menuSelection == 1)
                {
                    decimal? balance = apiService.GetBalance();
                    consoleService.PrintBalance(balance);
                }
                else if (menuSelection == 2)
                {
                    List<Transfer> pastTransfers = apiService.GetPastTransfers();
                    consoleService.PrintTransfers(pastTransfers);

                    int transferId = consoleService.PromptForTransferId();
                    if (transferId == 0)
                    {
                        MenuSelection();
                    }
                    bool bobsBool = false;
                    while (bobsBool == false && transferId != 0)
                    {
                        foreach (Transfer transfer in pastTransfers)
                        {
                            if (transfer.TransferId.Equals(transferId))
                            {
                                bobsBool = true;
                                break;
                            }
                        }
                        if (bobsBool == false)
                        {
                            dyn.Add("Transfer ID is not valid. Please enter a valid ID: ");
                            consoleService.PrintTransfers(pastTransfers);
                            transferId = consoleService.PromptForTransferId();
                        }
                    }
                    if(transferId == 0)
                    {
                        MenuSelection();
                    }
                    consoleService.PrintTransferById(apiService.GetTransferDetailsById(transferId));

                }
                else if (menuSelection == 3)
                {
                    // View my pending transfer requests.
                }
                else if (menuSelection == 4)
                {                  
                    try
                    {
                        List<User> users = apiService.GetAllUsers();
                        if (users != null && users.Count > 0)
                        {
                            consoleService.PrintUsers(users);
                            int userId = consoleService.PromptForUserId();
                            if (userId == 0)
                            {
                                MenuSelection();
                            }
                            bool bobsBool = false;
                            while (bobsBool == false && userId != 0)
                            {
                                foreach (User user in users)
                                {
                                    if (user.UserId.Equals(userId))
                                    {
                                        bobsBool = true;
                                        break;
                                    }
                                }
                                if (bobsBool == false)
                                {
                                    dyn.Add("User ID is not valid. Please enter a valid ID: ");
                                    consoleService.PrintUsers(users);
                                    userId = consoleService.PromptForUserId();
                                }
                            }
                            if (userId == 0)
                            {
                                MenuSelection();
                            }
                            decimal xferAmount = consoleService.PromptForAmount();
                            if (xferAmount != 0)
                            {
                                apiService.TransferMoney(userId, xferAmount);
                            }
                            
                            
                        }
                    }
                    catch (Exception ex)
                    {
                        dyn.Add(ex.Message);
                    }
                }
                else if (menuSelection == 5)
                {
                    // Request TE Bucks from another user.
                }
                else if (menuSelection == 6)
                {
                    dyn.Add("");
                    UserService.SetLogin(new ApiUser()); //wipe out previous login info
                    Console.Clear();
                    menuSelection = 0;
                }
                else
                {
                    dyn.Add("Goodbye!");
                    Environment.Exit(0);
                }
            }
        }
    }
}
