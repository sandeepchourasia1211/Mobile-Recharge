using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileRecharge.Application.Interfaces
{
    public interface IPaymentService
    {
        bool ProcessPayment(int userId, decimal amount);
    }
}
