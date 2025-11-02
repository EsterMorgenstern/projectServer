using BLL.Models;
using BLL.Services;

namespace BLL.Api
{
    public interface IBLLStudentHealthFund
    {
        List<BLLStudentHealthFundPerfect> Get();
        Task Create(BLLStudentHealthFund studentHealthFund);
        public BLLStudentHealthFund GetById(int id);
        public void Delete(int id);
        public void Update(BLLStudentHealthFund studentHealthFund);
        // הוספת תאריך לרשימת התאריכים שדווחו
        void AddReportedDate(int studentHealthFundId, DateTime date);

        // שליפת רשימת תאריכים שדווחו
        List<DateTime> GetReportedDates(int studentHealthFundId);

        // שליפת רשימת תאריכים שלא דווחו
        List<DateTime> GetUnreportedDates(int studentHealthFundId);
        // העברת תאריך מרשימת התאריכים שלא דווחו לרשימת התאריכים שדווחו
        Task ReportUnreportedDate(int studentHealthFundId, DateTime date);

        void UploadFile(int studentHealthFundId, string filePath, string fileType);
        Task<UnreportedTreatmentsSyncResult> ValidateAndFixUnreportedTreatments();

    }
}
