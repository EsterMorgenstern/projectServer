using DAL.Models;

namespace DAL.Api
{
    public interface IDALReportedDate: IDALCRUD<ReportedDate>
    {

        // שליפת רשימת תאריכים שדווחו
        List<ReportedDate> GetByStudentHealthFundId(int studentHealthFundId);
    }
}
