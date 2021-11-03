using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;
using System.Data.SqlClient;

namespace TenmoServer.DAO
{
    public class AccountSQLDao : IAccountSQLDao
    {
        private readonly string connectionString;
        const decimal startingBalance = 1000;

        public AccountSQLDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public Account GetAccountById(int accountId)
        {
            Account searchAccount = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"SELECT account_id, user_id, balance from accounts 
                                                        where account_id = @accountId", conn);

                    cmd.Parameters.AddWithValue("@accountId", accountId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        searchAccount = GetAccountFromReader(reader);
                    }
                }                
            }
            catch (SqlException ex)
            {
                throw;
            }
            return searchAccount;
        }

        public List<Transfer> GetAllTransfersForAccount(int accountId)
        {
            List<Transfer> listOfTransfers = new List<Transfer>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"SELECT transfer_id, transfer_type_id,  transfer_status_id,
                                                        account_from, account_to, amount
                                                        where account_from = @accountId OR account_to = @accountId", conn);

                    cmd.Parameters.AddWithValue("@accountId", accountId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Transfer transfer = GetTransferFromReader(reader);
                        listOfTransfers.Add(transfer);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
            return listOfTransfers;
        }

        private Account GetAccountFromReader(SqlDataReader reader)
        {
            Account account = new Account()
            {
                AccountId = Convert.ToInt32(reader["account_id"]),
                UserId = Convert.ToInt32(reader["user_id"]),
                Balance = Convert.ToDecimal(reader["balance"])
            };
            return account;
        }

        private Transfer GetTransferFromReader(SqlDataReader reader)
        {
            Transfer transfer = new Transfer()
            {
                TransferId = Convert.ToInt32(reader["transfer_id"]),
                TransferTypeId = Convert.ToInt32(reader["transfer_type_id"]),
                TransferStatusId = Convert.ToInt32(reader["transfer_status_id"]),
                AccountFrom = Convert.ToInt32(reader["account_from"]),
                AccountTo = Convert.ToInt32(reader["account_to"]),
                Amount = Convert.ToDecimal(reader["amount"])
            };
            return transfer;
        }
    }
}
