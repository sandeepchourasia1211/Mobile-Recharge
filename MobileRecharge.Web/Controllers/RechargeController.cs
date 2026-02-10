using Microsoft.AspNetCore.Mvc;
using MobileRecharge.Application.Interfaces;
using MobileRecharge.Domain.DTOs;

namespace MobileRecharge.Web.Controllers
{
    public class RechargeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly IRechargeService _rechargeService;

        public RechargeController(IRechargeService rechargeService)
        {
            _rechargeService = rechargeService;
        }

      
        [HttpPost]
        public IActionResult DoRecharge(RechargeRequestDto request)
        {
            var result = _rechargeService.DoRecharge(request);
            return View("Result", result);
        }
    }
}
