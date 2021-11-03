namespace TenmoServer.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public decimal Balance { get; set; }

        public Account(int accountId, int userId, decimal balance)
        {
            this.AccountId = accountId;
            this.UserId = userId;
            this.Balance = balance;
        }
    }
}
