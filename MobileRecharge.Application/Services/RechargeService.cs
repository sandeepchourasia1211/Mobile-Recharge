using MobileRecharge.Application.Interfaces;
using MobileRecharge.Domain.DTOs;
using MobileRecharge.Domain.Entities;

namespace MobileRecharge.Application.Services
{
    public class RechargeService : IRechargeService
    {
        private readonly IRechargeRepository _rechargeRepo;
        private readonly IUserRepository _userRepository;

        public RechargeService(
            IRechargeRepository rechargeRepo,
            IUserRepository userRepository)
        {
            _rechargeRepo = rechargeRepo;
            _userRepository = userRepository;
        }

        public RechargeResponseDto DoRecharge(RechargeRequestDto request, string paymentMethod)
        {
            // 1. Get UserId using Mobile Number
            int userId = _userRepository.GetUserIdByMobile(request.MobileNumber);

           
            // Auto-create user if not exists
            if (userId <= 0)
            {
                userId = _userRepository.CreateUserByMobile(request.MobileNumber);
            }

            // 2. Create new request with UserId
            var dbRequest = new CreateRechargeDto
            {
                UserId = userId,
                OperatorId = request.OperatorId,
                Amount = request.Amount
            };

            // 3. Create Recharge
            decimal walletBalance = _userRepository.GetWalletBalanceByUserId(userId);

            // Only block if user selected WALLET
            if (paymentMethod == PaymentMethods.WALLET)
            {
                walletBalance = _userRepository.GetWalletBalanceByUserId(userId);
                if (walletBalance < request.Amount)
                {
                    return new RechargeResponseDto
                    {
                        IsSuccess = false,
                        Message = "Insufficient wallet balance. Please choose UPI, Card, or NetBanking."
                    };
                }
            }

            // If payment is external (UPI / CARD / NETBANKING)
            // we DO NOT check wallet here


            // Wallet cannot pay → use external payment
            return new RechargeResponseDto
            {
                IsSuccess = false,
                Message = "Wallet balance is insufficient. Please choose another payment method.",
                RequiresExternalPayment = true
            };
        }
    }
}
