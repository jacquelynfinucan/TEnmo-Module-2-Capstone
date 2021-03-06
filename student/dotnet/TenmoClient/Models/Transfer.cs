using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace TenmoClient.Models
{
    public class Transfer
    {

        [Range(3001, double.PositiveInfinity, ErrorMessage = "Transfer id must be OVER 3000.")]
        public int TransferId { get; set; }

        [Required(ErrorMessage = "Transfer Type Required.")]
        [Range(1, 2, ErrorMessage = "Transfer Type Id must be 1 or 2.")]
        public int TransferTypeId { get; set; }

        [Range(1, 3, ErrorMessage = "Transfer Status Id must be 1, 2 or 3.")]
        public int TransferStatusId { get; set; }

        [Required(ErrorMessage = "Sender account Id Required.")]
        public int AccountFrom { get; set; }

        [Required(ErrorMessage = "Receiver account Id Required.")]
        public int AccountTo { get; set; }

        [Range(.01, double.PositiveInfinity, ErrorMessage = "Transfer amount cannot be 0 or negative.")]
        public decimal Amount { get; set; }

        public int Sender { get; set; }
        public string fromUser { get; set; }
        public string toUser { get; set; }

        public Transfer(int transferId, int transferTypeId, int transferStatusId, int accountFrom, int accountTo, decimal amount)
        {
            this.TransferId = transferId;
            this.TransferTypeId = transferTypeId;
            this.TransferStatusId = transferStatusId;
            this.AccountFrom = accountFrom;
            this.AccountTo = accountTo;
            this.Amount = amount;
        }
        public Transfer(int accountFrom, int accountTo, decimal amount)
        {
            this.AccountFrom = accountFrom;
            this.AccountTo = accountTo;
            this.Amount = amount;
        }
        public Transfer()
        {

        }
    }
}
