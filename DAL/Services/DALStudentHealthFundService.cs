using DAL.Api;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Services
{
    public class DALStudentHealthFundService : IDALStudentHealthFund
    {
        private readonly dbcontext dbcontext;

        public DALStudentHealthFundService(dbcontext context)
        {
            dbcontext = context;
        }

        public async Task<List<StudentHealthFund>> GetAll()
        {
            return await dbcontext.StudentHealthFunds
                .Where(x => x.IsActive)
                .AsNoTracking()
                .ToListAsync();
        }

        public StudentHealthFund GetById(int id)
        {
            var studentHealthFund = dbcontext.StudentHealthFunds.SingleOrDefault(x => x.Id == id);
            if (studentHealthFund == null)
            {
                throw new KeyNotFoundException($"StudentHealthFund with ID {id} not found.");
            }
            return studentHealthFund;
        }

        public async Task<StudentHealthFund?> GetActiveByStudentId(int studentId)
        {
            return await dbcontext.StudentHealthFunds
                .FirstOrDefaultAsync(x => x.StudentId == studentId && x.IsActive);
        }

        public async Task Create(StudentHealthFund studentHealthFund)
        {
            await dbcontext.StudentHealthFunds.AddAsync(studentHealthFund);
            await dbcontext.SaveChangesAsync();
        }

        public async Task Delete(int studentHealthFundId)
        {
            var studentHealthFund = await dbcontext.StudentHealthFunds
                .SingleOrDefaultAsync(x => x.Id == studentHealthFundId);

            if (studentHealthFund != null)
            {
                dbcontext.StudentHealthFunds.Remove(studentHealthFund);
                await dbcontext.SaveChangesAsync();
            }
        }

        public async Task Update(StudentHealthFund studentHealthFund)
        {
            dbcontext.StudentHealthFunds.Update(studentHealthFund);
            await dbcontext.SaveChangesAsync();
        }

        public void SaveFilePath(int studentHealthFundId, string filePath, string fileType)
        {
            var studentHealthFund = dbcontext.StudentHealthFunds.SingleOrDefault(x => x.Id == studentHealthFundId);
            if (studentHealthFund == null)
            {
                throw new KeyNotFoundException($"StudentHealthFund with ID {studentHealthFundId} not found.");
            }

            if (fileType == "Referral")
            {
                studentHealthFund.ReferralFilePath = filePath;
            }
            else if (fileType == "Commitment")
            {
                studentHealthFund.CommitmentFilePath = filePath;
            }
            else
            {
                throw new ArgumentException("Invalid file type.");
            }

            dbcontext.SaveChanges();
        }
    }
}