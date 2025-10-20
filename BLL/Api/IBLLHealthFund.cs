using System.Collections.Generic;
using BLL.Models;

namespace BLL.Api
{
    public interface IBLLHealthFund
    {
        List<BLLHealthFund> Get();
        void Create(BLLHealthFund healthFund);
        BLLHealthFund GetById(int id);
        void Delete(int id);
        void Update(BLLHealthFund healthFund);
    }
}
