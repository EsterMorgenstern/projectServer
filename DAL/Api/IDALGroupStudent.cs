using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Api
{
    public interface IDALGroupStudent
    {
        List<GroupStudent> Get();
        void Create(GroupStudent groupStudent);
        GroupStudent GetById(int id);
        void Delete(GroupStudent groupStudent);
        void Update(GroupStudent groupStudent);
    }
}
