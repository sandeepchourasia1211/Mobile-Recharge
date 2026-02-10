using MobileRecharge.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileRecharge.Application.Services
{
    public class PaymentService : IPaymentService
    {
        public bool ProcessPayment(int userId, decimal amount)
        {
            return true;
        }
    }
}
