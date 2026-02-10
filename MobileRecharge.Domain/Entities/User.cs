using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileRecharge.Domain.Entities
{
    internal class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public decimal WalletBalance { get; set; }
    }
}
