using Microsoft.AspNetCore.Mvc;
using TenmoServer.DAO;
using TenmoServer.Models;
using TenmoServer.Security;
using System.Collections.Generic;

namespace TenmoServer.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountSQLDao accountDao;
        private readonly IUserDao userDao;

        public AccountController(IAccountSQLDao _accountDao, IUserDao _userDao)
        {
            userDao = _userDao;
            accountDao = _accountDao;
        }

       [HttpGet("{accountId}")]
       public decimal GetBalance(int accountId)
        {
            Account account = accountDao.GetAccountById(accountId);
            decimal balance = account.Balance;
            return balance;
        }
       [HttpPost("transfers/{accountFrom}")]
       public bool SendTEBucks(int accountFrom, int accountTo, decimal amount)
        {
            bool isTransferSuccessful = false;
            Transfer transfer = new Transfer(2, 2, accountFrom, accountTo, amount);
            Transfer newTransfer = accountDao.CreateATransfer(transfer);

            try
            {
                accountDao.UpdateBalance(newTransfer.AccountFrom, -newTransfer.Amount); //amt is negative since it's being subtracted
                accountDao.UpdateBalance(newTransfer.AccountTo, newTransfer.Amount); //amt is positive since it's being added
                accountDao.UpdateATransferStatus(newTransfer.TransferId, 2); //sets status to Approved
                isTransferSuccessful = true;
            }  
            catch
            {
                accountDao.UpdateATransferStatus(newTransfer.TransferId, 3); //sets status to Rejected & transfer is not successful
            }
            
            return isTransferSuccessful;
        }

        [HttpGet("users")]
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

        [HttpGet("transfers")]
        //use ?accountId=int query 
       public List<Transfer> ShowUserTransfers(int accountId)
       {
            List<Transfer> listOfTransfers = accountDao.GetAllTransfersForAccount(accountId);
            return listOfTransfers;
       }

        [HttpGet("transfers/{transferId}")]
        public Transfer ShowATransfer(int transferId)
        {
            Transfer transfer = accountDao.GetTransferById(transferId);
            return transfer;
        }

    }
}
