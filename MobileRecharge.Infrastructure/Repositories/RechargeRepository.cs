using Dapper;
using Microsoft.Data.SqlClient;
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

        public int CreateRecharge(CreateRechargeDto request)
        {
            using var conn = _db.CreateConnection();

            try
            {
                return conn.ExecuteScalar<int>(
                    "sp_CreateRecharge",
                    new
                    {
                        request.UserId,
                        request.OperatorId,
                        request.Amount
                    },
                    commandType: CommandType.StoredProcedure);
            }
            catch (SqlException ex) when (ex.Message.Contains("Insufficient wallet balance"))
            {
                return -1; // business failure code
            }
        }


        //public int CreateRecharge(RechargeRequestDto request)
        //{
        //    using var conn = _db.CreateConnection();
        //    return conn.ExecuteScalar<int>(
        //        "sp_CreateRecharge",
        //        request,
        //        commandType: CommandType.StoredProcedure);
        //}
    }
}
