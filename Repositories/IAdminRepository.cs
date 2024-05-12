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
        public List<AssignmentModel> GetAssignments();
        public List<TreeModel> GetAllStudents();
        public List<TreeModel> GetTreeStudents();
        public void AddAssignment(AssignmentModel aa);

        public void UpdateTask(AssignmentModel assignment);
        public void DeleteTask(int id);

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