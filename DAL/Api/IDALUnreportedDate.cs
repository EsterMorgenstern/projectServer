using DAL.Models;

namespace DAL.Api
{
    public interface IDALUnreportedDate: IDALCRUD<UnreportedDate>
    {

        // שליפת רשימת תאריכים שלא דווחו
        List<UnreportedDate> GetByStudentHealthFundId(int studentHealthFundId);

    }
}
