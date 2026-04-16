using DAL.Api;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Services
{
    public class DALHealthFundCommitmentService : IDALHealthFundCommitment
    {
        private readonly dbcontext dbcontext;

        public DALHealthFundCommitmentService(dbcontext context)
        {
            dbcontext = context;
        }

        public List<HealthFundCommitment> GetByStudentHealthFundId(int studentHealthFundId)
        {
            return dbcontext.HealthFundCommitments
                .Where(x => x.StudentHealthFundId == studentHealthFundId)
                .AsNoTracking()
                .ToList();
        }

        public async Task Create(HealthFundCommitment commitment)
        {
            await dbcontext.HealthFundCommitments.AddAsync(commitment);
            await dbcontext.SaveChangesAsync();
        }

        public async Task Update(HealthFundCommitment commitment)
        {
            dbcontext.HealthFundCommitments.Update(commitment);
            await dbcontext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var entity = await dbcontext.HealthFundCommitments.SingleOrDefaultAsync(x => x.Id == id);
            if (entity != null)
            {
                dbcontext.HealthFundCommitments.Remove(entity);
                await dbcontext.SaveChangesAsync();
            }
        }

        public HealthFundCommitment GetById(int id)
        {
            var entity = dbcontext.HealthFundCommitments.SingleOrDefault(x => x.Id == id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"HealthFundCommitment with ID {id} not found.");
            }

            return entity;
        }
    }
}