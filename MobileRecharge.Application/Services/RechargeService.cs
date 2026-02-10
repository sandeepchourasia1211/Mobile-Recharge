using MobileRecharge.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileRecharge.Application.Services
{
    public class RechargeService : IRechargeService
    {
        private readonly IRechargeRepository _rechargeRepo;
        private readonly IPaymentService _paymentService;

        public RechargeService(
            IRechargeRepository rechargeRepo,
            IPaymentService paymentService)
        {
            _rechargeRepo = rechargeRepo;
            _paymentService = paymentService;
        }

        public RechargeResponseDto DoRecharge(RechargeRequestDto request)
        {
            bool paymentStatus = _paymentService.ProcessPayment(request.UserId, request.Amount);

            if (!paymentStatus)
                return new RechargeResponseDto { IsSuccess = false, Message = "Payment Failed" };

            int rechargeId = _rechargeRepo.CreateRecharge(request);

            return new RechargeResponseDto
            {
                IsSuccess = true,
                Message = "Recharge Successful",
                RechargeId = rechargeId
            };
        }
    }
}
