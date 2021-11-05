using System;
using System.Collections.Generic;
using TenmoClient.Models;
using RestSharp.Authenticators;

namespace TenmoClient
{
    class Program
    {
        private static readonly ConsoleService consoleService = new ConsoleService();
        private static readonly AuthService authService = new AuthService();
        private static readonly ApiService apiService = new ApiService("https://localhost:44315/",authService.getClient);
        private static readonly ConsoleService console = new ConsoleService();


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
                    Console.WriteLine("Welcome to TEnmo!");
                    Console.WriteLine("1: Login");
                    Console.WriteLine("2: Register");
                    Console.Write("Please choose an option: ");

                    if (!int.TryParse(Console.ReadLine(), out loginRegister))
                    {
                        Console.WriteLine("Invalid input. Please enter only a number.");
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
                                Console.WriteLine("");
                                Console.WriteLine("Registration successful. You can now log in.");
                                loginRegister = -1; //reset outer loop to allow choice for login
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid selection.");
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
                Console.WriteLine("");
                Console.WriteLine("Welcome to TEnmo! Please make a selection: ");
                Console.WriteLine("1: View your current balance");
                Console.WriteLine("2: View your past transfers");
                Console.WriteLine("3: View your pending requests");
                Console.WriteLine("4: Send TE bucks");
                Console.WriteLine("5: Request TE bucks");
                Console.WriteLine("6: Log in as different user");
                Console.WriteLine("0: Exit");
                Console.WriteLine("---------");
                Console.Write("Please choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out menuSelection))
                {
                    Console.WriteLine("Invalid input. Please enter only a number.");
                }
                else if (menuSelection == 1)
                {
                    decimal? balance = apiService.GetBalance();
                    console.PrintBalance(balance);
                }
                else if (menuSelection == 2)
                {
                    List<Transfer> pastTransfers = apiService.GetPastTransfers();
                    console.PrintTransfers(pastTransfers);

                    int transferId = console.PromptForTransferId();
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
                            Console.WriteLine("Transfer ID is not valid. Please enter a valid ID: ");
                            console.PrintTransfers(pastTransfers);
                            transferId = console.PromptForTransferId();
                        }
                    }
                    if(transferId == 0)
                    {
                        MenuSelection();
                    }
                    console.PrintTransferById(apiService.GetTransferDetailsById(transferId));

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
                            console.PrintUsers(users);
                            int userId = console.PromptForUserId();
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
                                    Console.WriteLine("User ID is not valid. Please enter a valid ID: ");
                                    console.PrintUsers(users);
                                    userId = console.PromptForUserId();
                                }
                            }
                            if (userId == 0)
                            {
                                MenuSelection();
                            }
                            decimal xferAmount = console.PromptForAmount();

                            apiService.TransferMoney(userId, xferAmount);
                            
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else if (menuSelection == 5)
                {
                    // Request TE Bucks from another user.
                }
                else if (menuSelection == 6)
                {
                    Console.WriteLine("");
                    UserService.SetLogin(new ApiUser()); //wipe out previous login info
                    Console.Clear();
                    menuSelection = 0;
                }
                else
                {
                    Console.WriteLine("Goodbye!");
                    Environment.Exit(0);
                }
            }
        }
    }
}
