using BLL.Models;

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

    }
}
