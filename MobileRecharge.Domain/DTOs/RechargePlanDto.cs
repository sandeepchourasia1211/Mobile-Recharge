using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileRecharge.Domain.DTOs
{
    public class RechargePlanDto
    {
        public int PlanId { get; set; }
        public string PlanName { get; set; }
        public decimal Amount { get; set; }
        public int ValidityDays { get; set; }
        public string Description { get; set; }
    }
}
