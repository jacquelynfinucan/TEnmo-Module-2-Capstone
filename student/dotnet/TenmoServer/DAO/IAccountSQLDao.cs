using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface IAccountSQLDao
    {
        Account GetAccountById(int accountId);

        List<Transfer> GetAllTransfersForAccount(int accountId);

        Transfer GetTransferById(int transferId);

        Transfer CreateATransfer(Transfer transfer);

        Account UpdateBalance(int accountId, int amount);

    }
}
