using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileRecharge.Domain.DTOs
{
    public class RechargeResponseDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int RechargeId { get; set; }
    }
}
