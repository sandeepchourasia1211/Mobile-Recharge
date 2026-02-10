using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileRecharge.Domain.Entities
{
    internal class Recharge
    {
        public int RechargeId { get; set; }
        public int UserId { get; set; }
        public int OperatorId { get; set; }
        public decimal Amount { get; set; }
        public DateTime RechargeDate { get; set; }
    }
}
