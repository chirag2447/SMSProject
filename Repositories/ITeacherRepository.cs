using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mvc.Models;

namespace SMSProject.Repositories
{
    public interface ITeacherRepository
    {
        List<StudentModel> GetAllData();
    }
}