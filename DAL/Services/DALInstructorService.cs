using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal.Models;
using DAL.Api;
using Microsoft.EntityFrameworkCore;

namespace DAL.Services
{
    public class DALInstructorService:IDALInstructor
    {
        dbcontext dbcontext;
        public DALInstructorService(dbcontext data)
        {
            dbcontext = data;
        }
    }
}
