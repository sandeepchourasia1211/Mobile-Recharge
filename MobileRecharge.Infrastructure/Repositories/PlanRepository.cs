using Dapper;
using MobileRecharge.Application.Interfaces;
using MobileRecharge.Domain.DTOs;
using MobileRecharge.Infrastructure.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileRecharge.Infrastructure.Repositories
{
    public class PlanRepository : IPlanRepository
    {
        private readonly DbConnectionFactory _db;

        public PlanRepository(DbConnectionFactory db)
        {
            _db = db;
        }

        public List<RechargePlanDto> GetPlansByOperator(int operatorId)
        {
            using var conn = _db.CreateConnection();

            return conn.Query<RechargePlanDto>(
                @"SELECT PlanId, PlanName, Amount, ValidityDays, Description
                  FROM RechargePlans
                  WHERE OperatorId = @OperatorId AND IsActive = 1",
                new { OperatorId = operatorId }
            ).ToList();
        }

        public RechargePlanDto GetPlanById(int planId)
        {
            using var conn = _db.CreateConnection();

            return conn.QueryFirstOrDefault<RechargePlanDto>(
                @"SELECT PlanId, PlanName, Amount, ValidityDays, Description
                  FROM RechargePlans
                  WHERE PlanId = @PlanId",
                new { PlanId = planId });
        }
    }
}
