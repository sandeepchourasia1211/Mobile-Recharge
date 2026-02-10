using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileRecharge.Domain.DTOs
{
    internal class RechargeRequestDto
    {
        public int UserId { get; set; }
        public int OperatorId { get; set; }
        public decimal Amount { get; set; }
    }
}
