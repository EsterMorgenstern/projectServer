using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Api
{
    public interface IDALStudentHealthFund:IDALCRUD<StudentHealthFund>
    {
        StudentHealthFund GetById(int id);
        List<ReportedDate> GetReportedDates(int studentHealthFundId);
        List<UnreportedDate> GetUnreportedDates(int studentHealthFundId);


    }
}
