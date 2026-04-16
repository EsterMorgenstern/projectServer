using BLL.Models;
using BLL.Services;

namespace BLL.Api
{
    public interface IBLLStudentHealthFund
    {
        List<BLLStudentHealthFundPerfect> Get();
        BLLStudentHealthFund GetById(int id);
        Task Create(BLLStudentHealthFund studentHealthFund);
        void Update(BLLStudentHealthFund studentHealthFund);
        void Delete(int id);

        List<DateTime> GetReportedDates(int studentHealthFundId);
        List<DateTime> GetUnreportedDates(int studentHealthFundId);
        void AddReportedDate(int studentHealthFundId, DateTime date);
        Task ReportUnreportedDate(int studentHealthFundId, DateTime date);

        void UploadFile(int studentHealthFundId, string filePath, string fileType);
        Task<UnreportedTreatmentsSyncResult> ValidateAndFixUnreportedTreatments();
    }
}