using Microsoft.AspNetCore.Mvc;
using MobileRecharge.Application.Interfaces;
using MobileRecharge.Domain.DTOs;

namespace MobileRecharge.Web.Controllers
{
    public class RechargeController : Controller
    {
        private readonly IRechargeService _rechargeService;
        private readonly IPlanRepository _planRepository;

        public RechargeController(
            IRechargeService rechargeService,
            IPlanRepository planRepository)
        {
            _rechargeService = rechargeService;
            _planRepository = planRepository;
        }

        // =========================
        // RECHARGE HOME
        // =========================
        public IActionResult Index()
        {
            return View();
        }

        // =========================
        // LOAD PLANS (AJAX)
        // =========================
        [HttpGet]
        public IActionResult GetPlans(int operatorId)
        {
            var plans = _planRepository.GetPlansByOperator(operatorId);
            return Json(plans);
        }

        // =========================
        // SUBMIT RECHARGE
        // =========================
        [HttpPost]
        public IActionResult DoRecharge(RechargeRequestDto request, string PaymentMethod)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(PaymentMethod))
            {
                ModelState.AddModelError("", "Please complete all required fields");
                return View("Index", request);
            }

            // =========================
            // CASE 1: WALLET PAYMENT
            // =========================
            if (PaymentMethod == "WALLET")
            {
                var result = _rechargeService.DoRecharge(request, "WALLET");

                if (!result.IsSuccess)
                {
                    ModelState.AddModelError("", result.Message);
                    return View("Index", request);
                }

                return View("Result", result);
            }

            // =========================
            // CASE 2: UPI PAYMENT
            // =========================
            if (PaymentMethod == "UPI")
            {
                return RedirectToAction(
                    "Upi",
                    "Payment",
                    new PaymentRequestDto
                    {
                        MobileNumber = request.MobileNumber,
                        OperatorId = request.OperatorId,
                        Amount = request.Amount,
                        PaymentMethod = "UPI"
                    });
            }

            // =========================
            // CASE 3: CARD / NET BANKING (future)
            // =========================
            ModelState.AddModelError("", "Selected payment method is not supported yet.");
            return View("Index", request);
        }
    }
}
