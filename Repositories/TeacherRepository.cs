using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using mvc.Models;
using Npgsql;
using SMSProject.Models;

namespace SMSProject.Repository
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly string _conn;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TeacherRepository(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _conn = configuration.GetConnectionString("SMSProject");
            _httpContextAccessor = httpContextAccessor;
        }


        public List<TreeModel> GetAllStudents()
        {
            try
            {
                using (var con = new NpgsqlConnection(_conn))
                {
                    con.Open();
                    using (var cmd = new NpgsqlCommand("SELECT * FROM t_student", con))
                    {
                        var reader = cmd.ExecuteReader();
                        List<TreeModel> students = new List<TreeModel>();
                        while (reader.Read())
                        {
                            var student = new TreeModel
                            {
                                c_id = reader.GetInt32(0),
                                c_userid = reader.IsDBNull(1) ? (int?)null : reader.GetInt32(1),
                                c_first_name = reader.GetString(2),
                                c_last_name = reader.GetString(3),
                                c_dob = reader.GetDateTime(4),
                                c_gender = reader.GetString(5),
                                c_age = reader.GetInt32(6),
                                c_address = reader.GetString(7),
                                c_contact_number = reader.GetString(8),
                                c_profile = reader.GetString(9),
                                c_password = reader.GetString(10)
                            };
                            students.Add(student);
                        }
                        return students;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}