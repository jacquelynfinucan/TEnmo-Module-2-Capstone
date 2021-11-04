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

        public AccountController(IAccountSQLDao _accountDao)
        {
            accountDao = _accountDao;
        }

       [HttpGet("balance")]
       public static decimal GetBalance()
        {
            return 0.0M;
        }
       [HttpPost("/transfers/{userid}")]
       public static void SendTEBucks()
        {
            //Transfer newTransfer = 
        }

        [HttpGet("/transfers/users")]
        public static List<User> GetAllUsers()
        {
            return null;
        }

        [HttpGet("/transfers")]
       public List<Transfer> ShowUserTransfers(int accountId)
       {
            List<Transfer> listOfTransfers = accountDao.GetAllTransferForAccount(accountId);
            return listOfTransfers;
       }
        [HttpGet("/transfers/{transid}")]
        public static Transfer ShowATransfer()
        {
            return null;
        }

    }
}
