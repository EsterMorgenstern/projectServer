using System.Collections.Generic;
using System.Linq;
using BLL.Api;
using BLL.Models;
using DAL.Api;
using DAL.Models;

namespace BLL.Services
{
    public class BLLHealthFundService : IBLLHealthFund
    {
        private readonly IDAL dal;

        public BLLHealthFundService(IDAL dal)
        {
            this.dal = dal;
        }

        public List<BLLHealthFund> Get()
        {
            var healthFunds = dal.HealthFunds.Get();
            return healthFunds.Select(hf => new BLLHealthFund
            {
                HealthFundId = hf.HealthFundId,
                Name = hf.Name,
                FundType = hf.FundType,
                MaxTreatmentsPerYear = hf.MaxTreatmentsPerYear,
                PricePerLesson = hf.PricePerLesson,
                MonthlyPrice = hf.MonthlyPrice,
                RequiresReferral = hf.RequiresReferral,
                RequiresCommitment = hf.RequiresCommitment,
                IsActive = hf.IsActive,
                ValidUntilAge = hf.ValidUntilAge ?? 0, 
                EligibilityDetails = hf.EligibilityDetails
            }).ToList();
        }

        public void Create(BLLHealthFund healthFund)
        {
            var hf = new HealthFund
            {
                HealthFundId = healthFund.HealthFundId,
                Name = healthFund.Name,
                FundType = healthFund.FundType,
                MaxTreatmentsPerYear = healthFund.MaxTreatmentsPerYear,
                PricePerLesson = healthFund.PricePerLesson,
                MonthlyPrice = healthFund.MonthlyPrice,
                RequiresReferral = healthFund.RequiresReferral,
                RequiresCommitment = healthFund.RequiresCommitment,
                IsActive = healthFund.IsActive,
                ValidUntilAge=healthFund.ValidUntilAge,
                EligibilityDetails=healthFund.EligibilityDetails
            };
            dal.HealthFunds.Create(hf);
        }

        public BLLHealthFund GetById(int id)
        {
            var hf = dal.HealthFunds.GetById(id);
            return new BLLHealthFund
            {
                HealthFundId = hf.HealthFundId,
                Name = hf.Name,
                FundType = hf.FundType,
                MaxTreatmentsPerYear = hf.MaxTreatmentsPerYear,
                PricePerLesson = hf.PricePerLesson,
                MonthlyPrice = hf.MonthlyPrice,
                RequiresReferral = hf.RequiresReferral,
                RequiresCommitment = hf.RequiresCommitment,
                IsActive = hf.IsActive,
                ValidUntilAge = hf.ValidUntilAge ?? 0, 
                EligibilityDetails = hf.EligibilityDetails
            };
        }

        public void Delete(int id)
        {
            dal.HealthFunds.Delete(id);
        }

        public void Update(BLLHealthFund healthFund)
        {
            var hf = dal.HealthFunds.GetById(healthFund.HealthFundId);
            if (hf != null)
            {
                hf.Name = healthFund.Name;
                hf.FundType = healthFund.FundType;
                hf.MaxTreatmentsPerYear = healthFund.MaxTreatmentsPerYear;
                hf.PricePerLesson = healthFund.PricePerLesson;
                hf.MonthlyPrice = healthFund.MonthlyPrice;
                hf.RequiresReferral = healthFund.RequiresReferral;
                hf.RequiresCommitment = healthFund.RequiresCommitment;
                hf.IsActive = healthFund.IsActive;
                hf.ValidUntilAge = healthFund.ValidUntilAge;
                hf.EligibilityDetails = healthFund.EligibilityDetails;

                dal.HealthFunds.Update(hf);
            }
            else
            {
                throw new KeyNotFoundException($"HealthFund with ID {healthFund.HealthFundId} not found.");
            }
        }
    }
}
