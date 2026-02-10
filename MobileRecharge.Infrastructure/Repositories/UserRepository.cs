using Dapper;
using MobileRecharge.Infrastructure.Db;
using MobileRecharge.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileRecharge.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbConnectionFactory _db;

        public UserRepository(DbConnectionFactory db)
        {
            _db = db;
        }

        public decimal GetWalletBalanceByUserId(int userId)
        {
            using var conn = _db.CreateConnection();

            return conn.ExecuteScalar<decimal>(
                "SELECT WalletBalance FROM Users WHERE UserId = @UserId",
                new { UserId = userId });
        }

        public int GetUserIdByMobile(string mobileNumber)
        {
            using var conn = _db.CreateConnection();
            return conn.ExecuteScalar<int>(
                "SELECT UserId FROM Users WHERE MobileNumber = @MobileNumber AND IsActive = 1",
                new { MobileNumber = mobileNumber });
        }
        public int CreateUserByMobile(string mobileNumber)
        {
            using var conn = _db.CreateConnection();

            return conn.ExecuteScalar<int>(
                @"
                IF EXISTS (SELECT 1 FROM Users WHERE MobileNumber = @MobileNumber)
                BEGIN
                  SELECT UserId FROM Users WHERE MobileNumber = @MobileNumber
                END
                ELSE
                BEGIN
                INSERT INTO Users (FullName, MobileNumber, IsActive)
                OUTPUT INSERTED.UserId
                VALUES ('New User', @MobileNumber, 1)
                END
                ",
                new { MobileNumber = mobileNumber });
        }



        public bool DeductBalance(int userId, decimal amount)
        {
            using var connection = _db.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId);
            parameters.Add("@Amount", amount);

            int rowsAffected = connection.Execute(
                "sp_DeductWalletBalance",
                parameters,
                commandType: CommandType.StoredProcedure);

            return rowsAffected > 0;
        }
    }
}
