using DAL.Api;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Services
{
    public class DALReportedDateService : IDALReportedDate
    {
        private readonly dbcontext _dbContext;

        public DALReportedDateService(dbcontext dbContext)
        {
            _dbContext = dbContext;
        }

        // שליפת רשימת תאריכים שדווחו לפי StudentHealthFundId
        public List<ReportedDate> GetByStudentHealthFundId(int studentHealthFundId)
        {
            var results = _dbContext.ReportedDates
                .Where(rd => rd.StudentHealthFundId == studentHealthFundId)
                .ToList();

            return results ?? new List<ReportedDate>();
        }


        // יצירת רשומה חדשה בטבלת ReportedDates
        public async Task Create(ReportedDate reportedDate)
        {
            await _dbContext.ReportedDates.AddAsync(reportedDate);
            await _dbContext.SaveChangesAsync();
        }

        // מחיקת רשומה מטבלת ReportedDates לפי ID
        public async Task Delete(int reportedDateId)
        {
            var reportedDate = await _dbContext.ReportedDates.FindAsync(reportedDateId);
            if (reportedDate != null)
            {
                _dbContext.ReportedDates.Remove(reportedDate);
                await _dbContext.SaveChangesAsync();
            }
        }

        // עדכון רשומה בטבלת ReportedDates
        public async Task Update(ReportedDate reportedDate)
        {
            _dbContext.ReportedDates.Update(reportedDate);
            await _dbContext.SaveChangesAsync();
        }

        // שליפת כל הרשומות בטבלת ReportedDates
        public async Task<List<ReportedDate>> GetAll()
        {
            return await _dbContext.ReportedDates.ToListAsync();
        }
    }
}
