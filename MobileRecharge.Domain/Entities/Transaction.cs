using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileRecharge.Domain.Entities
{
    internal class Transaction
    {
        public int TransactionId { get; set; }
        public int RechargeId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
