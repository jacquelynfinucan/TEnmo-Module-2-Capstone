using System.ComponentModel.DataAnnotations;


namespace TenmoServer.Models
{
    public class Account
    {
        [Required(ErrorMessage = "Account Id is required.")]
        public int AccountId { get; set; }
        [Required(ErrorMessage = "User Id is required.")]
        public int UserId { get; set; }
        [Required(ErrorMessage ="User balance is required")]
        [Range(0,double.PositiveInfinity,ErrorMessage = "User balance cannot be negative.")]
        public decimal Balance { get; set; }

        public Account(int accountId, int userId, decimal balance) 
        {
            this.AccountId = accountId;
            this.UserId = userId;
            this.Balance = balance;
        }
        public Account()
        {
            //had to add this empty constructor for GetAccountFromReader to work
        }
    }
}
