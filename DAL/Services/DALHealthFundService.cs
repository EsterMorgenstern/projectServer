using System.Collections.Generic;
using System.Linq;
using DAL.Api;
using DAL.Models;

namespace DAL.Services
{
    public class DALHealthFundService : IDALHealthFund
    {
        private readonly dbcontext dbcontext;

        public DALHealthFundService(dbcontext context)
        {
            dbcontext = context;
        }

        public List<HealthFund> Get()
        {
            return dbcontext.HealthFunds.ToList();
        }

        public void Create(HealthFund healthFund)
        {
            dbcontext.HealthFunds.Add(healthFund);
            dbcontext.SaveChanges();
        }

        public HealthFund GetById(int id)
        {
            var healthFund = dbcontext.HealthFunds.SingleOrDefault(x => x.HealthFundId == id);
            if (healthFund == null)
            {
                throw new KeyNotFoundException($"HealthFund with ID {id} not found.");
            }
            return healthFund;
        }

        public void Delete(int healthFundId)
        {
            var healthFund = dbcontext.HealthFunds.SingleOrDefault(x => x.HealthFundId == healthFundId);
            if (healthFund != null)
            {
                dbcontext.HealthFunds.Remove(healthFund);
                dbcontext.SaveChanges();
            }
        }

        public void Update(HealthFund healthFund)
        {
            dbcontext.HealthFunds.Update(healthFund);
            dbcontext.SaveChanges();
        }
    }
}
