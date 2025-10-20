using DAL.Api;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Services
{
    public class DALUnreportedDateService : IDALUnreportedDate
    {
        private readonly dbcontext _dbContext;

        public DALUnreportedDateService(dbcontext dbContext)
        {
            _dbContext = dbContext;
        }

        // שליפת רשימת תאריכים שלא דווחו לפי StudentHealthFundId
        public List<UnreportedDate> GetByStudentHealthFundId(int studentHealthFundId)
        {
            var results= _dbContext.UnreportedDates
                .Where(ud => ud.StudentHealthFundId == studentHealthFundId)
                .ToList();
            return results ?? new List<UnreportedDate>();
        }

        // יצירת רשומה חדשה בטבלת UnreportedDates
        public async Task Create(UnreportedDate unreportedDate)
        {
            await _dbContext.UnreportedDates.AddAsync(unreportedDate);
            await _dbContext.SaveChangesAsync();
        }

        // מחיקת רשומה מטבלת UnreportedDates לפי ID
        public async Task Delete(int unreportedDateId)
        {
            var unreportedDate = _dbContext.UnreportedDates.Find(unreportedDateId);
            if (unreportedDate != null)
            {
                _dbContext.UnreportedDates.Remove(unreportedDate);
                _dbContext.SaveChanges();
            }
        }

        // עדכון רשומה בטבלת UnreportedDates
        public async Task Update(UnreportedDate unreportedDate)
        {
            _dbContext.UnreportedDates.Update(unreportedDate);
            await _dbContext.SaveChangesAsync();
        }

        // שליפת כל הרשומות בטבלת UnreportedDates
        public async Task<List<UnreportedDate>> GetAll()
        {
            return await _dbContext.UnreportedDates.ToListAsync();
        }
    }
}
