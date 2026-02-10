using Dapper;
using MobileRecharge.Application.Interfaces;
using MobileRecharge.Domain.DTOs;
using MobileRecharge.Infrastructure.Db;
using System.Data;

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
            using var conn = _db.CreateConnection();
            return conn.ExecuteScalar<int>(
                "sp_CreateRecharge",
                request,
                commandType: CommandType.StoredProcedure);
        }
    }
}
