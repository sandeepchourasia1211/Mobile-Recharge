using MobileRecharge.Domain.DTOs;
using MobileRecharge.Infrastructure.Db;
using MobileRecharge.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileRecharge.Infrastructure.Repositories
{
    public class RechargeRepository : IRechargeRepository
    {
        private readonly DbConnectionFactory _db;

        public RechargeRepository(DbConnectionFactory db)
        {
            _db = db;
        }

        public int CreateRecharge(RechargeRequestDto request)
        {
            //using var conn = _db.CreateConnection();
            //return conn.ExecuteScalar<int>(
            //    "sp_CreateRecharge",
            //    request,
            //    commandType: System.Data.CommandType.StoredProcedure);
            return 0;
        }

        
    }
}
