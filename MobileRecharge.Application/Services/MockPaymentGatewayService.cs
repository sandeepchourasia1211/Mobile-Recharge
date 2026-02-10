using MobileRecharge.Application.Interfaces;
using MobileRecharge.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileRecharge.Application.Services
{
    public class MockPaymentGatewayService : IPaymentGatewayService
    {
        public PaymentResponseDto ProcessPayment(PaymentRequestDto request)
        {
            // Simulate payment success
            return new PaymentResponseDto
            {
                IsSuccess = true,
                TransactionId = Guid.NewGuid().ToString(),
                Message = $"{request.PaymentMethod} payment successful"
            };
        }
    }
}
