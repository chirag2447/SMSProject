using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mvc.Models;
using SMSProject.Models;

namespace SMSProject.Repository
{
    public interface IAdminRepository
    {
        public List<TreeModel> GetAllStudents();

        bool Insert(StudentModel stud);
        List<StudentModel> GetAllData();
        void multidelete(List<int> ids);
        bool Update(StudentModel book);
        void Delete(int id);
        List<StudentModel> SearchStudents(string query);
        List<StudentModel> GetDataPagination(int pageNumber, int pageSize);
        List<UserModel> GetAllTeacherData();
    }
}