using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Api
{
    public interface IDALUser
    {
        List<User> Get();
        void Create(User user);
        User GetById(int id);
        void Delete(int userId);
        void Update(User user);
    }
}
