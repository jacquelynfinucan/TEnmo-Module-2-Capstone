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
                                                  WHERE transfer_id = {id}", conn);
                cmd.Parameters.AddWithValue("id", accountId);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new Transfer(reader["transfer_id", "transfer_type_id", "transfer_status_id", "account_from"])


                }



            }
        }


    }
}
