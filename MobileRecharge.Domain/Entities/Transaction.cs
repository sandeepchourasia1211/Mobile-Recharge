using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileRecharge.Domain.Entities
{
    internal class Transaction
    {
        public int WalletTxnId { get; set; }
        public int UserId { get; set; }
        public int? RechargeId { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; } // CREDIT / DEBIT
        public decimal BalanceAfterTxn { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
