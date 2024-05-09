using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using mvc.Models;
using Npgsql;

namespace SMSProject.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly string _conn;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AdminRepository(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _conn = configuration.GetConnectionString("SMSProject");
            _httpContextAccessor = httpContextAccessor;
        }
        public void Delete(int id)
        {

            try
            {
                using NpgsqlConnection conn = new NpgsqlConnection(_conn);
                conn.Open();
                using NpgsqlCommand cmd = new NpgsqlCommand("delete from t_student where c_id = @c_studid", conn);
                cmd.Parameters.AddWithValue("@c_studid", id);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        public List<StudentModel> GetAllData()
        {
            List<StudentModel> datas = new List<StudentModel>();
            try
            {
                using NpgsqlConnection conn = new NpgsqlConnection(_conn);
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT c_id, c_userid, c_first_name , c_last_name , c_dob , c_gender , c_age , c_address, c_contactno , c_profile , c_password FROM t_student", conn))
                {
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

        public bool Insert(StudentModel stud)
        {
            try
            {
                var password = stud.c_first_name + stud.c_dob;
                using NpgsqlConnection conn = new NpgsqlConnection(_conn);
                conn.Open();

                using NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO t_student (c_userid,c_first_name,  c_last_name, c_dob, c_gender,c_age,c_address,c_contactno,c_profile,c_password) VALUES (@c_userid,@c_first_name,  @c_last_name, @c_dob, @c_gender,@c_age,@c_address,@c_contactno,@c_profile,@c_password)", conn);
                cmd.Parameters.AddWithValue("@c_userid", stud.c_userid);
                cmd.Parameters.AddWithValue("@c_first_name", stud.c_first_name);
                cmd.Parameters.AddWithValue("@c_last_name", stud.c_last_name);
                cmd.Parameters.AddWithValue("@c_dob", stud.c_dob);
                cmd.Parameters.AddWithValue("@c_gender", stud.c_gender);
                cmd.Parameters.AddWithValue("@c_age", stud.c_age);
                cmd.Parameters.AddWithValue("@c_address", stud.c_address);
                cmd.Parameters.AddWithValue("@c_contactno", stud.c_contactno);
                cmd.Parameters.AddWithValue("@c_profile", stud.c_profile);
                cmd.Parameters.AddWithValue("@c_password", password);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                return true;



            }
            catch (Exception e)
            {
                Console.WriteLine("Error inserting data: " + e.Message);
                return false;
            }
        }

        public void multidelete(List<int> ids)
        {
            try
            {
                using NpgsqlConnection conn = new NpgsqlConnection(_conn);
                conn.Open();
                using NpgsqlCommand cmd = new NpgsqlCommand($"delete from t_student where c_id IN ({string.Join(",", ids)})", conn);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public bool Update(StudentModel stud)
        {
            try
            {
                using NpgsqlConnection conn = new NpgsqlConnection(_conn);
                conn.Open();
                using NpgsqlCommand cmd = new NpgsqlCommand("update t_student set c_userid=@c_userid,c_first_name=@c_first_name,c_last_name=@c_last_name,c_dob=@c_dob,c_gender=@c_gender,c_age=@c_age,c_address=@c_address,c_contactno=@c_contactno,c_profile=@c_profile,c_password=@c_password where c_id=@c_id", conn);
                cmd.Parameters.AddWithValue("@c_id", stud.c_id);
                cmd.Parameters.AddWithValue("@c_userid", stud.c_userid);
                cmd.Parameters.AddWithValue("@c_first_name", stud.c_first_name);
                cmd.Parameters.AddWithValue("@c_last_name", stud.c_last_name);
                cmd.Parameters.AddWithValue("@c_dob", stud.c_dob);
                cmd.Parameters.AddWithValue("@c_gender", stud.c_gender);
                cmd.Parameters.AddWithValue("@c_age", stud.c_age);
                cmd.Parameters.AddWithValue("@c_address", stud.c_address);
                cmd.Parameters.AddWithValue("@c_contactno", stud.c_contactno);
                cmd.Parameters.AddWithValue("@c_profile", stud.c_profile);
                cmd.Parameters.AddWithValue("@c_password", stud.c_password);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public List<StudentModel> SearchStudents(string query)
        {
            List<StudentModel> datas = new List<StudentModel>();
            try
            {
                using NpgsqlConnection conn = new NpgsqlConnection(_conn);
                conn.Open();
                using NpgsqlCommand cmd = new NpgsqlCommand($"SELECT c_id, c_userid, c_first_name, c_last_name, c_dob, c_gender, c_age, c_address, c_contactno, c_profile, c_password FROM t_student WHERE c_first_name LIKE '%{query}%' OR c_last_name LIKE '%{query}%'", conn);
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return datas;
        }

        public List<StudentModel> GetDataPagination(int pageNumber, int pageSize)
        {
            List<StudentModel> datas = new List<StudentModel>();
            try
            {
                using NpgsqlConnection conn = new NpgsqlConnection(_conn);
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand($"SELECT c_id, c_userid, c_first_name , c_last_name , c_dob , c_gender , c_age , c_address, c_contactno , c_profile , c_password FROM t_student LIMIT {pageSize} OFFSET {(pageNumber - 1) * pageSize}", conn))
                {
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