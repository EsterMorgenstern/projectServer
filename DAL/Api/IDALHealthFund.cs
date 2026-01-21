using DAL.Models;

namespace DAL.Api
{
    public interface IDALHealthFund
    {
        List<HealthFund> Get();
        void Create(HealthFund healthFund);
        HealthFund GetById(int id);
        void Delete(int healthFundId);
        void Update(HealthFund healthFund);
    }
}
