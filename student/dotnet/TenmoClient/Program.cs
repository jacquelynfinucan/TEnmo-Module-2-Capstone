using System;
using System.Collections.Generic;
using TenmoClient.Models;
using RestSharp.Authenticators;
using System.Threading.Tasks;

namespace TenmoClient
{
    class Program
    {
        private static readonly AuthService authService = new AuthService();
        private static readonly ApiService apiService = new ApiService("https://localhost:44315/",authService.getClient);
        private static readonly ConsoleService consoleService = new ConsoleService(apiService);
        public static DynamicConsole dyn = new DynamicConsole();
        public static bool exitLogin = false;
        public static bool exit = false;
        static async Task Main(string[] args)
        {
            await Run();
            dyn.Clear();
            dyn.Add("Bye! Thanks for using TEnmo!");
            dyn[0].DoARainbow();
            await Task.Delay(10000);
        }

        private static async Task Run()
        {
            while(!exit)
            {
                int loginRegister = -1;
                while (loginRegister != 1 && loginRegister != 2)
                {
                    startAgain:
                    if (indexToReset != null)
                    {
                        await dyn[indexToReset.Value].ChangeColor(ConsoleColor.White, true);
                    }
                    if (dyn.Length < 3)
                    {
                        dyn.Add("Welcome to TEnmo!");
                        dyn.Add("1: Login");
                        dyn.Add("2: Register");
                    }
                        

                    if (!int.TryParse(dyn.ReadLine(), out loginRegister))
                    {
                        dyn.Add("Invalid input. Please enter only a number.");
                    }
                    else if (loginRegister == 1)
                    {
                        await RunDynAdjust(loginRegister);
                        while (!UserService.IsLoggedIn()) //will keep looping until user is logged in
                        {
                            if (exitLogin) { exitLogin = false; goto startAgain; }
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
                        await RunDynAdjust(loginRegister);

                        bool isRegistered = false;
                        while (!isRegistered) //will keep looping until user is registered
                        {
                            LoginUser registerUser = consoleService.PromptForLogin();
                            isRegistered = authService.Register(registerUser);
                            if (isRegistered)
                            {
                                ResetToBaseMenu(-5);
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
                dyn.Clear();
                indexToReset = null;
                await MenuSelection();
            }
        }

        private static async Task RunDynAdjust(int menuSelected)
        {
                    Console.CursorVisible = false;
            if (indexToReset != null)
            {
                await dyn[indexToReset.Value].ChangeColor(ConsoleColor.White,true);
            }
            ResetToBaseMenu(-5);
            await dyn[menuSelected].ChangeColor(ConsoleColor.Green,true);
            indexToReset = menuSelected;
        }



        private static int? indexToReset = null; 
        private static async Task MenuSelection()
        {
            int menuSelection = -1;
            while (menuSelection != 0)
            {
                if (dyn.Length == 0)
                {
                    dyn.Add("");
                    dyn.Add("Welcome to TEnmo! Please make a selection: ");
                    dyn.Add("1: View your current balance");
                    dyn.Add("2: View your past transfers");
                   //dyn.Add("3: View your pending requests");
                     dyn.Add("3: Send TE bucks");
                   //dyn.Add("3: Request TE bucks");
                    dyn.Add("4: Log in as different user");
                    dyn.Add("0: Exit");
                    dyn.Add("---------");
                }

                if (!int.TryParse(dyn.ReadLine(), out menuSelection))
                {
                    dyn.Add("Invalid input. Please enter only a number.");
                }
                else if (menuSelection == 1)
                {
                    await MenuDynAdjust(menuSelection);
                    decimal? balance = apiService.GetBalance();
                    consoleService.PrintBalance(balance);
                }
                else if (menuSelection == 2)
                {
                   await  MenuDynAdjust(menuSelection);
                    List<Transfer> pastTransfers = apiService.GetPastTransfers();
                    consoleService.PrintTransfers(pastTransfers);

                    int transferId = consoleService.PromptForTransferId();
                    if (transferId == 0)
                    {
                        ResetToBaseMenu();
                        await MenuSelection();
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
                        await MenuSelection ();
                    }
                    consoleService.PrintTransferById(apiService.GetTransferDetailsById(transferId));

                }
                //else if (menuSelection == 3)
                //{
                //    // View my pending transfer requests.
                //}
                else if (menuSelection == 3)
                {
                    await MenuDynAdjust (menuSelection);

                    try
                    {
                        List<User> users = apiService.GetAllUsers();
                        if (users != null && users.Count > 0)
                        {
                            consoleService.PrintUsers(users);
                            int userId = consoleService.PromptForUserId();
                            if (userId == 0)
                            {
                                await MenuSelection ();
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
                                    //consoleService.PrintUsers(users);
                                    userId = consoleService.PromptForUserId();
                                }
                            }
                            if (userId == 0)
                            {
                                await MenuSelection();
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
                //else if (menuSelection == 5)
                //{
                //    // Request TE Bucks from another user.
                //}
                else if (menuSelection == 4)
                {
                    await MenuDynAdjust(menuSelection);

                    dyn.Add("");
                    UserService.SetLogin(new ApiUser()); //wipe out previous login info
                    dyn.Clear();
                    indexToReset = null;
                    menuSelection = 0;
                }
                else if(menuSelection == 0)
                {
                    exit = true;
                }
            }
        }

        private static async Task MenuDynAdjust(int menuSelected)
        {
            Console.CursorVisible = false;
            if (indexToReset != null)
            {
                await dyn[indexToReset.Value].ChangeColor(ConsoleColor.White,true);
            }
            ResetToBaseMenu();
            await dyn[menuSelected + 1].ChangeColor(ConsoleColor.Green,true);
            indexToReset = menuSelected + 1;
        }
        public static void ResetToBaseMenu(int offset = 0)
        {
            if (dyn.Length > 8+offset)
            {
                dyn.Remove(dyn.Length - (8+offset));
            }
        }
    }
}
