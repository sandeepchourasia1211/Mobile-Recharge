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

        // ===================== UPI =====================
        [HttpGet]
        public IActionResult Upi(PaymentRequestDto model)
        {
            return View(model);
        }

        [HttpPost]
        public IActionResult ProcessUpi(PaymentRequestDto model, string UpiId)
        {
            if (string.IsNullOrEmpty(UpiId))
            {
                ModelState.AddModelError("", "UPI ID is required");
                return View("Upi", model);
            }

            return CompletePaymentAndRecharge(model);
        }

        // ===================== CARD =====================
        [HttpGet]
        public IActionResult Card(PaymentRequestDto model)
        {
            return View(model);
        }

        [HttpPost]
        public IActionResult ProcessCard(PaymentRequestDto model)
        {
            return CompletePaymentAndRecharge(model);
        }

        // ===================== NET BANKING =====================
        [HttpGet]
        public IActionResult NetBanking(PaymentRequestDto model)
        {
            return View(model);
        }

        [HttpPost]
        public IActionResult ProcessNetBanking(PaymentRequestDto model)
        {
            return CompletePaymentAndRecharge(model);
        }

        // ===================== COMMON =====================
        private IActionResult CompletePaymentAndRecharge(PaymentRequestDto model)
        {
            var paymentResult = _paymentGatewayService.ProcessPayment(model);

            if (!paymentResult.IsSuccess)
            {
                ModelState.AddModelError("", paymentResult.Message);
                return View();
            }

            var rechargeResult = _rechargeService.DoRecharge(
                new RechargeRequestDto
                {
                    MobileNumber = model.MobileNumber,
                    OperatorId = model.OperatorId,
                    Amount = model.Amount
                },
                model.PaymentMethod);

            return View("~/Views/Recharge/Result.cshtml", rechargeResult);
        }
    }
}
