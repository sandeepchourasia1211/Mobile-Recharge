using MobileRecharge.Application.Interfaces;
using MobileRecharge.Domain.DTOs;

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

        public RechargeResponseDto DoRecharge(RechargeRequestDto request)
        {
            // 1. Get UserId using Mobile Number
            int userId = _userRepository.GetUserIdByMobile(request.MobileNumber);

            if (userId <= 0)
            {
                return new RechargeResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid mobile number"
                };
            }

            // 2. Create new request with UserId
            var dbRequest = new RechargeRequestDto
            {
                UserId = userId,
                OperatorId = request.OperatorId,
                Amount = request.Amount
            };

            // 3. Create Recharge
            int rechargeId = _rechargeRepo.CreateRecharge(dbRequest);

            return new RechargeResponseDto
            {
                IsSuccess = true,
                Message = "Recharge Successful",
                RechargeId = rechargeId
            };
        }
    }
}
