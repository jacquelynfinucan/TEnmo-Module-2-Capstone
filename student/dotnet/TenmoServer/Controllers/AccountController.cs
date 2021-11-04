using Microsoft.AspNetCore.Mvc;
using TenmoServer.DAO;
using TenmoServer.Models;
using TenmoServer.Security;
using System.Collections.Generic;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountSQLDao accountDao;
        private readonly IUserDao userDao;

        public AccountController(IAccountSQLDao _accountDao, IUserDao _userDao)
        {
            userDao = _userDao;
        }

       [HttpGet("/{accountId}")]
       public decimal GetBalance(int accountId)
        {
            Account account = accountDao.GetAccountById(accountId);
            decimal balance = account.Balance;
            return balance;
        }
       [HttpPost("/transfers/{userid}")]
       public static void SendTEBucks()
        {
            Transfer newTransfer = 
        }

        [HttpGet("/users")]
        public List<User> GetAllUsersIdsAndNames()
        {
            List<User> allUsers = userDao.GetUsers();
            List<User> userIdsAndNames = new List<User>();
            foreach (User user in allUsers)
            {
                User newUser = new User();
                newUser.UserId = user.UserId;
                newUser.Username = user.Username;
                userIdsAndNames.Add(newUser);
            }
            return userIdsAndNames;
        }

        [HttpGet("/transfers")]
       public List<Transfer> ShowUserTransfers(int accountId)
       {
            List<Transfer> listOfTransfers = accountDao.GetAllTransfersForAccount(accountId);
            return listOfTransfers;
       }

        [HttpGet("/transfers/{transid}")]
        public Transfer ShowATransfer(int transferId)
        {
            Transfer transfer = accountDao.GetTransferById(transferId);
            return transfer;
        }

    }
}
