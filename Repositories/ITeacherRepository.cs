using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mvc.Models;
using SMSProject.Models;

namespace SMSProject.Repository
{
    public interface ITeacherRepository
    {
        public List<TreeModel> GetAllStudents();

    }
}