using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileRecharge.Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        bool DeductBalance(int userId, decimal amount);
    }
}
