using BLL.Api;
using BLL.Models;
using DAL.Api;
using Microsoft.AspNetCore.Mvc;

namespace server.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentHealthFundController : ControllerBase // Change base class to ControllerBase
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
        [HttpPost("{id}/ReportUnreportedDate")]
        public void ReportUnreportedDate(int id, [FromBody] DateTime date)
        {
            studentHealthFunds.ReportUnreportedDate(id, date);
        }

        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile file, int studentHealthFundId, string fileType)
        {
            try
            {
                if (file == null)
                    return BadRequest("File not received");

                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", studentHealthFundId.ToString());
                Directory.CreateDirectory(folderPath);

                var filePath = Path.Combine(folderPath, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                studentHealthFunds.UploadFile(studentHealthFundId, filePath, fileType);

                return Ok(new { FilePath = filePath });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }

    }
}

