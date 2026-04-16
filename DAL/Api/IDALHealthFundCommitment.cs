using DAL.Models;

namespace DAL.Api
{
    public interface IDALHealthFundCommitment
    {
        List<HealthFundCommitment> GetByStudentHealthFundId(int studentHealthFundId);
        Task Create(HealthFundCommitment commitment);
        Task Update(HealthFundCommitment commitment);
        Task Delete(int id);
        HealthFundCommitment GetById(int id);
    }
}