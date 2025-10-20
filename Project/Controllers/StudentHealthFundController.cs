using BLL.Api;
using BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace server.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentHealthFundController
    {
        private readonly IBLLStudentHealthFund studentHealthFunds;

        public StudentHealthFundController(IBLL manager)
        {
            studentHealthFunds = manager.StudentHealthFunds;
        }

        [HttpGet("GetAll")]
        public List<BLLStudentHealthFundPerfect> Get()
        {
            return studentHealthFunds.Get();
        }

        [HttpGet("getById/{id}")]
        public BLLStudentHealthFund GetById(int id)
        {
            return studentHealthFunds.GetById(id);
        }

        [HttpPost("Add")]
        public void Create(BLLStudentHealthFund studentHealthFund)
        {
            studentHealthFunds.Create(studentHealthFund);
        }

        [HttpPut("Update")]
        public void Update(BLLStudentHealthFund studentHealthFund)
        {
            studentHealthFunds.Update(studentHealthFund);
        }

        [HttpDelete("Delete")]
        public void Delete(int studentHealthFundId)
        {
            studentHealthFunds.Delete(studentHealthFundId);
        }
        [HttpPost("{id}/report-date")]
        public void ReportDate(int id, [FromBody] DateTime date)
        {
            studentHealthFunds.AddReportedDate(id, date);

        }

        [HttpGet("{id}/reported-dates")]
        public List<DateTime> GetReportedDates(int id)
        {
            return studentHealthFunds.GetReportedDates(id);
        }

        [HttpGet("{id}/unreported-dates")]
        public List<DateTime> GetUnreportedDates(int id)
        {
           return studentHealthFunds.GetUnreportedDates(id);
        }

    }
}

