using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mvc.Models;
using Npgsql;

namespace SMSProject.Repositories
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

        public List<StudentModel> GetAllData()
        {
            List<StudentModel> datas = new List<StudentModel>();
            try
            {
                using NpgsqlConnection conn = new NpgsqlConnection(_conn);
                conn.Open();
                // int teacherUserId = _httpContextAccessor.HttpContext.Session.GetInt32("userid") ?? 0; 
                int teacherUserId = _httpContextAccessor.HttpContext.Session.GetInt32("userid") ?? 0; 
                string query = "SELECT c_id, c_userid, c_first_name, c_last_name, c_dob, c_gender, c_age, c_address, c_contactno, c_profile, c_password FROM t_student WHERE c_userid = @TeacherUserId";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TeacherUserId", teacherUserId);
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            StudentModel data = new StudentModel
                            {
                                c_id = Convert.ToInt32(reader["c_id"]),
                                c_userid = Convert.ToInt32(reader["c_userid"]),
                                c_first_name = reader["c_first_name"].ToString(),
                                c_last_name = reader["c_last_name"].ToString(),
                                c_dob = Convert.ToDateTime(reader["c_dob"]),
                                c_gender = reader["c_gender"].ToString(),
                                c_age = Convert.ToInt32(reader["c_age"]),
                                c_address = reader["c_address"].ToString(),
                                c_contactno = reader["c_contactno"].ToString(),
                                c_profile = reader["c_profile"].ToString(),
                                c_password = reader["c_password"].ToString(),
                            };
                            datas.Add(data);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return datas;
        }

    }
}