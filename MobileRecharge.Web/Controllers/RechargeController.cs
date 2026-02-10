using Microsoft.AspNetCore.Mvc;
using MobileRecharge.Application.Interfaces;
using MobileRecharge.Domain.DTOs;

namespace MobileRecharge.Web.Controllers
{
    public class RechargeController : Controller
    {
        private readonly IPlanRepository _planRepository;
        public RechargeController(IRechargeService rechargeService,IPlanRepository planRepository)
        {
            _rechargeService = rechargeService;
            _planRepository = planRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
        private readonly IRechargeService _rechargeService;

        [HttpGet]
        public IActionResult GetPlans(int operatorId)
        {
            var plans = _planRepository.GetPlansByOperator(operatorId);
            return Json(plans);
        }

        [HttpPost]
        public IActionResult DoRecharge(RechargeRequestDto request)
        {
            //if (!ModelState.IsValid)
            //    return View("Index", request);

            //RechargeResponseDto rechargeId = _rechargeService.DoRecharge(request);
            //ViewBag.RechargeId = rechargeId;

            //return View("Result");

            var result = _rechargeService.DoRecharge(request);
            return View("Result", result);
        }
    }
}
