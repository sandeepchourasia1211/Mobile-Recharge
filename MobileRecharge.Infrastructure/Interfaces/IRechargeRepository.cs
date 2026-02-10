using MobileRecharge.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileRecharge.Infrastructure.Interfaces
{
    public interface IRechargeRepository
    {
        int CreateRecharge(RechargeRequestDto request);
    }
}
