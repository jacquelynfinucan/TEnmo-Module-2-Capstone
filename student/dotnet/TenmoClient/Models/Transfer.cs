using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Models
{
    public class Transfer
    {
        //public int TransferId { get; set; }
        //public int TransferTypeId { get; set; }
        //public int TransferStatusId { get; set; }
        public int AccountFrom { get; set; }
        public int AccountTo { get; set; }
        public decimal Amount { get; set; }

        public Transfer(int accountFrom, int accountTo, decimal amount)
        {
            //this.TransferId = transferId;
            //this.TransferTypeId = transferTypeId;
            //this.TransferStatusId = transferStatusId;
            this.AccountFrom = accountFrom;
            this.AccountTo = accountTo;
            this.Amount = amount;
        }
        //public Transfer()
        //{
          
        //}

    }
}
