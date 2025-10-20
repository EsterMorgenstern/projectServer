using BLL.Api;
using BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace server.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthFundController
    {
        private readonly IBLLHealthFund healthFunds;

        public HealthFundController(IBLL manager)
        {
            healthFunds = manager.HealthFunds;
        }

        [HttpGet("GetAll")]
        public List<BLLHealthFund> Get()
        {
            return healthFunds.Get();
        }

        [HttpGet("getById/{id}")]
        public BLLHealthFund GetById(int id)
        {
            return healthFunds.GetById(id);
        }

        [HttpPost("Add")]
        public void Create(BLLHealthFund healthFund)
        {
            healthFunds.Create(healthFund);
        }

        [HttpPut("Update")]
        public void Update(BLLHealthFund healthFund)
        {
            healthFunds.Update(healthFund);
        }

        [HttpDelete("Delete")]
        public void Delete(int healthFundId)
        {
            healthFunds.Delete(healthFundId);
        }
    }
}

