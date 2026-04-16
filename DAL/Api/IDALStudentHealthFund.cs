using DAL.Models;

namespace DAL.Api
{
    public interface IDALStudentHealthFund
    {
        Task<List<StudentHealthFund>> GetAll();
        StudentHealthFund GetById(int id);
        Task<StudentHealthFund?> GetActiveByStudentId(int studentId);
        Task Create(StudentHealthFund studentHealthFund);
        Task Update(StudentHealthFund studentHealthFund);
        Task Delete(int studentHealthFundId);
        void SaveFilePath(int studentHealthFundId, string filePath, string fileType);
    }
}