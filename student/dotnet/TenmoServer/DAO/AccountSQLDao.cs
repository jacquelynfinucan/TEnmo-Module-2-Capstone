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

        public Transfer GetTransferById(int TransferId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(@"SELECT transfer_id,transfer_type_id,transfer_status_id,account_from,account_to,amount
                                                  FROM transfers
                                                  WHERE transfer_id = @id", conn);
                cmd.Parameters.AddWithValue("@id", TransferId);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return GetTransferFromReader(reader);
                }
            }
            return null;
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
                                                        FROM transfers
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

        public Transfer CreateATransfer(Transfer transfer)
        {

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"INSERT INTO transfers (transfer_type_id,transfer_status_id,account_from,account_to,amount)
                                                      OUTPUT inserted.transfer_id
                                                      VALUES ({transfer_type_id},{transfer_status_id},{account_from},{account_to},{amount})", conn);

                    cmd.Parameters.AddWithValue("transfer_type_id", transfer.TransferTypeId);
                    cmd.Parameters.AddWithValue("transfer_status_id", transfer.TransferStatusId);
                    cmd.Parameters.AddWithValue("account_from", transfer.AccountFrom);
                    cmd.Parameters.AddWithValue("account_to", transfer.AccountTo);
                    cmd.Parameters.AddWithValue("amount", transfer.Amount);

                    transfer.TransferId = (int)cmd.ExecuteScalar();

                    return transfer;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            
        }

        public Account UpdateBalance(int accountId, int moneyToAdd)
        {
            var balance = GetAccountById(accountId).Balance;
            var newBalance = balance + moneyToAdd;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"UPDATE accounts
                                                      //SET balance = @newBalance
                                                      WHERE account_id = @account_id", conn);

                    cmd.Parameters.AddWithValue("@newBalance",newBalance );
                    cmd.Parameters.AddWithValue("@account_id", accountId);
          

                    return GetAccountById(accountId);
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

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
