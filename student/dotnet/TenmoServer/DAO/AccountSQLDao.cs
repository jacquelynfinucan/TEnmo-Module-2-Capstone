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

        public Transfer GetTransferById(int transferId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(@"SELECT transfer_id,transfer_type_id,transfer_status_id,account_from,account_to,amount,1 as Sender,usersfrom.username as name_from,usersto.username as name_to
                                                  FROM transfers
                                                  LEFT JOIN accounts as fromaccount on account_from = fromaccount.account_id
                                                  LEFT JOIN accounts as toaccount on account_to = toaccount.account_id
                                                  
                                                  LEFT JOIN users as usersfrom on usersfrom.user_id = fromaccount.user_id
                                                  LEFT JOIN users as usersto on usersto.user_id = toaccount.user_id
                                                  
                                                  WHERE transfer_id = @id", conn);
                cmd.Parameters.AddWithValue("@id", transferId);

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
                throw ex;
            }
            return searchAccount;
        }

        public  List<Transfer> GetAllTransfersForAccount(int accountId)
        {
            List<Transfer> listOfTransfers = new List<Transfer>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"SELECT transfer_id, transfer_type_id,  transfer_status_id, account_from, account_to, amount, 1 as Sender,usersfrom.username as name_from,usersto.username as name_to
                                                      FROM transfers 
                                                      
                                                      LEFT JOIN accounts as fromaccount on account_from = account_id
                                                      LEFT JOIN accounts as toaccount on toaccount.account_id = account_to
                                                      
                                                      LEFT JOIN users as usersfrom on usersfrom.user_id = fromaccount.user_id
                                                      LEFT JOIN users as usersto on usersto.user_id = toaccount.user_id
                                                      
                                                      WHERE account_from = 2001
                                                      
                                                      UNION
                                                      
                                                      SELECT transfer_id, transfer_type_id,  transfer_status_id, account_from, account_to, amount, 0 as Sender,usersfrom.username as name_from,usersto.username as name_to
                                                      FROM transfers 
                                                      
                                                      LEFT JOIN accounts as fromaccount on account_from = account_id
                                                      LEFT JOIN accounts as toaccount on toaccount.account_id = account_to
                                                      
                                                      LEFT JOIN users as usersfrom on usersfrom.user_id = fromaccount.user_id
                                                      LEFT JOIN users as usersto on usersto.user_id = toaccount.user_id
                                                      WHERE account_to = 2001", conn);

                    cmd.Parameters.AddWithValue("@accountId", accountId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Transfer transfer = GetTransferFromReader(reader);
                        
                        listOfTransfers.Add(transfer);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
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
                                                      VALUES (@transfer_type_id,@transfer_status_id,@account_from,@account_to,@amount)", conn);

                    cmd.Parameters.AddWithValue("@transfer_type_id", transfer.TransferTypeId);
                    cmd.Parameters.AddWithValue("@transfer_status_id", transfer.TransferStatusId);
                    cmd.Parameters.AddWithValue("@account_from", transfer.AccountFrom);
                    cmd.Parameters.AddWithValue("@account_to", transfer.AccountTo);
                    cmd.Parameters.AddWithValue("@amount", transfer.Amount);

                    transfer.TransferId = (int)cmd.ExecuteScalar();

                    return transfer;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public bool UpdateATransferStatus(int transferId, int transferStatusId)
        {
            bool isUpdateSuccessful = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"UPDATE transfers SET transfer_status_id = @transferStatusId
                                                      where transfer_type_id = @transferId)", conn);

                    cmd.Parameters.AddWithValue("@transferId", transferId);
                    cmd.Parameters.AddWithValue("@transferStatusId", transferStatusId);
                    isUpdateSuccessful = true;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return isUpdateSuccessful;
        }

        public Account UpdateBalance(int accountId, decimal moneyToAdd)
        {
            var balance = GetAccountById(accountId).Balance;
            var newBalance = balance + moneyToAdd;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"UPDATE accounts
                                                      SET balance = @newBalance
                                                      WHERE account_id = @account_id", conn);

                    cmd.Parameters.AddWithValue("@newBalance", newBalance);
                    cmd.Parameters.AddWithValue("@account_id", accountId);

                    cmd.ExecuteNonQuery();

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
            Transfer transfer = new Transfer((int)reader["transfer_type_id"], (int)reader["transfer_status_id"], 
                (int)reader["account_from"], (int)reader["account_to"], (decimal)reader["amount"]);
            transfer.TransferId = Convert.ToInt32(reader["transfer_id"]);
            transfer.Sender = (int)reader["Sender"];
            transfer.toUser = (string)reader["name_to"];
            transfer.fromUser = (string)reader["name_from"];
            return transfer;
        }

        public int GetAccountIdFromUserId(int userId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"Select account_id from accounts
                                                      JOIN users on users.user_id = accounts.user_id
                                                      WHERE users.user_id = @userId", conn);

                    cmd.Parameters.AddWithValue("@userId", userId);

                    int accountId = (int)cmd.ExecuteScalar();
                    return accountId;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public string GetUserNameFromAccountId(int accountId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"Select username from users
                                                      JOIN accounts on users.user_id = accounts.user_id
                                                      WHERE account_id = @accountId", conn);
                    cmd.Parameters.AddWithValue("@accountId", accountId);

                    string userName = (string)cmd.ExecuteScalar();
                    return userName;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

    }
}
