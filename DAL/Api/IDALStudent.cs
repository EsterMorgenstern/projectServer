using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Api
{
    public interface IDALStudent
    {
        List<Student> Get();
        void Create(Student item);
        Student GetById(int id);
        void Delete(Student student);
        void Update(Student student);
    }
}
