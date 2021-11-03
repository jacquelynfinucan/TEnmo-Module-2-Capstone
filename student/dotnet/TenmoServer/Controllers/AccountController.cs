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
       [HttpGet("/balance")]
       public static decimal GetBalance()
        {
            return 0;
        }
       [HttpPut("/transfers/{userid}")]
       public static void SendTEBucks()
        {

        }
       [HttpGet("/transfers")]
       public static List<Transfer> ShowUserTransfers()
       {
           return null;
       }
        [HttpGet("/transfers/{transid}")]
        public static Transfer ShowATransfer()
        {
            return null;
        }

    }
}
