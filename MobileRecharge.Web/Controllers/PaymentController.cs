using Microsoft.AspNetCore.Mvc;
using MobileRecharge.Application.Interfaces;
using MobileRecharge.Domain.DTOs;

namespace MobileRecharge.Web.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentGatewayService _paymentGatewayService;
        private readonly IRechargeService _rechargeService;

        public PaymentController(
            IPaymentGatewayService paymentGatewayService,
            IRechargeService rechargeService)
        {
            _paymentGatewayService = paymentGatewayService;
            _rechargeService = rechargeService;
        }

        [HttpGet]
        public IActionResult Upi(PaymentRequestDto model)
        {
            return View(model);
        }

        [HttpPost]
        public IActionResult ProcessUpiPayment(PaymentRequestDto model, string UpiId)
        {
            if (string.IsNullOrEmpty(UpiId))
            {
                ModelState.AddModelError("", "UPI ID is required");
                return View("Upi", model);
            }

            // Mock UPI validation
            var paymentResult = _paymentGatewayService.ProcessPayment(model);

            if (!paymentResult.IsSuccess)
            {
                ModelState.AddModelError("", paymentResult.Message);
                return View("Upi", model);
            }

            // Payment success → Recharge
            var rechargeResult = _rechargeService.DoRecharge(
                new RechargeRequestDto
                {
                    MobileNumber = model.MobileNumber,
                    OperatorId = model.OperatorId,
                    Amount = model.Amount
                },
                "UPI");

            return View("~/Views/Recharge/Result.cshtml", rechargeResult);
        }
    }
}
