namespace TenmoServer.Models
{
    public class Transfer
    {
        public int TransferId { get; set; }
        public int TransferTypeId { get; set; }
        public int TransferStatusId { get; set; } = 1; //default is pending, switches to approved when transfer completes
        public int AccountFrom { get; set; }
        public int AccountTo { get; set; }
        public decimal Amount { get; set; }

        public Transfer(int transferId, int transferTypeId, int transferStatusId, int accountFrom, int accountTo, decimal amount)
        {
            this.TransferId = transferId;
            this.TransferTypeId = transferTypeId;
            this.TransferStatusId = transferStatusId;
            this.AccountFrom = accountFrom;
            this.AccountTo = accountTo;
            this.Amount = amount;
        }
        public Transfer()
        {
            //had to add this empty constructor for GetTransferFromReader to work
        }
    }
}
