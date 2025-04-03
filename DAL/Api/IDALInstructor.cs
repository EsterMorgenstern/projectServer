using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Api
{
    public interface IDALInstructor
    {
        List<Instructor> Get();
        void Create(Instructor instructor);
        Instructor GetById(int id);
        void Delete(Instructor instructor);
        void Update(Instructor instructor);
    }
}
