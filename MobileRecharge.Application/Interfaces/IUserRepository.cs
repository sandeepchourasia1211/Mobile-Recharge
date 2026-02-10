using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileRecharge.Application.Interfaces
{
    public interface IUserRepository
    {
        int GetUserIdByMobile(string mobileNumber);
        int CreateUserByMobile(string mobileNumber);
        decimal GetWalletBalanceByUserId(int userId);

        bool DeductBalance(int userId, decimal amount);
    }
}
