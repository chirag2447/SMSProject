using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mvc.Models;

namespace SMSProject.Repository
{
    public interface IAdminRepository
    {
        bool Insert(StudentModel stud);
        List<StudentModel> GetAllData();
        void multidelete(List<int> ids);
        bool Update(StudentModel book);
        void Delete(int id);
    }
}