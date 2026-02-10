using MobileRecharge.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileRecharge.Application.Interfaces
{
    public interface IPlanRepository
    {
        List<RechargePlanDto> GetPlansByOperator(int operatorId);
        RechargePlanDto GetPlanById(int planId);
    }
}
