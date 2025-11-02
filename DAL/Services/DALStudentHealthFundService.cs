using DAL.Api;
using DAL.Models;

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
            return dbcontext.StudentHealthFunds.ToList();
        }

        public async Task Create(StudentHealthFund studentHealthFund)
        {
            dbcontext.StudentHealthFunds.Add(studentHealthFund);
            dbcontext.SaveChanges();
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

        public async Task Delete(int studentHealthFundId)
        {
            var studentHealthFund = dbcontext.StudentHealthFunds.SingleOrDefault(x => x.Id == studentHealthFundId);
            if (studentHealthFund != null)
            {
                dbcontext.StudentHealthFunds.Remove(studentHealthFund);
                dbcontext.SaveChanges();
            }
        }

        public async Task Update(StudentHealthFund studentHealthFund)
        {
            dbcontext.StudentHealthFunds.Update(studentHealthFund);
            dbcontext.SaveChanges();
        }
        public List<ReportedDate> GetReportedDates(int studentHealthFundId)
        {
            return dbcontext.ReportedDates
                .Where(rd => rd.StudentHealthFundId == studentHealthFundId)
                .ToList();
        }

        public List<UnreportedDate> GetUnreportedDates(int studentHealthFundId)
        {
            return dbcontext.UnreportedDates
                .Where(ud => ud.StudentHealthFundId == studentHealthFundId)
                .ToList();
        }

        public void AddReportedDate(ReportedDate reportedDate)
        {
            dbcontext.ReportedDates.Add(reportedDate);
            dbcontext.SaveChanges();
        }

        public void RemoveUnreportedDate(int unreportedDateId)
        {
            var unreportedDate = dbcontext.UnreportedDates.SingleOrDefault(ud => ud.Id == unreportedDateId);
            if (unreportedDate != null)
            {
                dbcontext.UnreportedDates.Remove(unreportedDate);
                dbcontext.SaveChanges();
            }
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
