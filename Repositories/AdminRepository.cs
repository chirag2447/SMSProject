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
    public class AdminRepository : IAdminRepository
    {
        private readonly string _conn;
        private readonly NpgsqlConnection conn;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AdminRepository(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _conn = configuration.GetConnectionString("SMSProject");
            conn = new NpgsqlConnection(configuration.GetConnectionString("SMSProject"));

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
                using (NpgsqlCommand cmd = new NpgsqlCommand(@"SELECT t_student.c_id, t_student.c_userid, t_student.c_first_name , t_student.c_last_name , t_student.c_dob , t_student.c_gender , t_student.c_age , t_student.c_address, t_student.c_contactno , t_student.c_profile , t_student.c_password , t_user.c_first_name FROM t_student join t_user on t_student.c_userid = t_user.c_id", conn))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            StudentModel data = new StudentModel
                            {
                                c_id = reader.GetInt32(0),
                                c_userid = reader.GetInt32(1),
                                c_first_name = reader.GetString(2),
                                c_last_name = reader.GetString(3),
                                c_dob = reader.GetDateTime(4),
                                c_gender = reader.GetString(5),
                                c_age = reader.GetInt32(6),
                                c_address = reader.GetString(7),
                                c_contactno = reader.GetString(8),
                                c_profile = reader.GetString(9),
                                c_password = reader.GetString(10),
                                c_user_first_name = reader.GetString(11),
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

        public List<UserModel> GetAllTeacherData()
        {
            List<UserModel> datas = new List<UserModel>();
            try
            {
                using NpgsqlConnection conn = new NpgsqlConnection(_conn);
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT c_first_name,c_id FROM t_user where c_role = 'User'", conn))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UserModel data = new UserModel
                            {
                                c_id = Convert.ToInt32(reader["c_id"]),
                                c_first_name = reader["c_first_name"].ToString(),
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

        public List<TreeModel> GetAllStudents()
        {
            try
            {
                using (var con = new NpgsqlConnection(_conn))
                {
                    con.Open();
                    using (var cmd = new NpgsqlCommand("SELECT * FROM tree", con))
                    {
                        var reader = cmd.ExecuteReader();
                        List<TreeModel> students = new List<TreeModel>();
                        while (reader.Read())
                        {
                            var student = new TreeModel
                            {
                                c_id = reader.GetInt32(0),
                                parentId = reader.IsDBNull(1) ? (int?)null : reader.GetInt32(1),
                                c_userid = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2),
                                c_first_name = reader.GetString(3),
                                c_last_name = reader.GetString(4),
                                c_gmail = reader.GetString(5),
                                c_password = reader.GetString(6),
                                c_phone = reader.GetString(7),
                                c_dob = reader.GetDateTime(8),
                                c_gender = reader.GetString(9),
                                c_age = reader.GetInt32(10),
                                c_country_id = reader.GetInt32(11),
                                c_state_id = reader.GetInt32(12),
                                c_city_id = reader.GetInt32(13),
                                c_address = reader.GetString(14),
                                c_language = reader.GetFieldValue<string[]>(15),
                                c_qualification = reader.GetString(16),
                                c_profile = reader.GetString(17),
                                c_role = reader.GetString(18),
                                c_contactno = reader.GetString(19)

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

        public List<TreeModel> GetTreeStudents()
        {
            //             try
            //             {
            //                 using (var con = new NpgsqlConnection(_conn))
            //                 {
            //                     con.Open();
            //                     var query = @"
            //     SELECT
            //         t_teacher.c_id AS id,
            //         NULL AS parentId,
            //         t_teacher.c_first_name AS first_name,
            //         t_teacher.c_last_name AS last_name,
            //         t_teacher.c_profile AS profile
            //     FROM
            //         public.t_user AS t_teacher
            //     UNION ALL
            //     SELECT
            //         t_student.c_id AS id,
            //         t_student.c_userid AS parentId,
            //         t_student.c_first_name AS first_name,
            //         t_student.c_last_name AS last_name,
            //         t_student.c_profile AS profile
            //     FROM
            //         public.t_student AS t_student;
            // ";
            //                     var cmd = new NpgsqlCommand(query, con);
            //                     var students = new List<TreeModel>();
            //                     var reader = cmd.ExecuteReader();
            //                     while (reader.Read())
            //                     {
            //                         var person = new TreeModel
            //                         {
            //                             id = reader.GetInt32(0),
            //                             parentId = reader.IsDBNull(1) ? (int?)null : reader.GetInt32(1),
            //                             first_name = reader.GetString(2),
            //                             last_name = reader.GetString(3),
            //                             profile = reader.GetString(4)
            //                         };
            //                         students.Add(person);
            //                     }
            //                     return students;

            //                 }
            //             }
            //             catch (Exception e)
            //             {
            //                 System.Console.WriteLine(e.Message);
            //             }
            return null;
        }


        public List<AssignmentModel> GetAssignments()
        {
            List<AssignmentModel> assignments = new List<AssignmentModel>();
            try
            {
                using (var con = new NpgsqlConnection(_conn))
                {
                    con.Open();
                    using (var cmd = new NpgsqlCommand("SELECT * FROM t_assignment", con))
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            var assignment = new AssignmentModel
                            {
                                id = reader.GetInt32(0),
                                title = reader.GetString(1),
                                start = reader.GetDateTime(2),
                                end = reader.GetDateTime(3),
                                percentComplete = reader.GetDouble(4) / 100
                            };
                            assignments.Add(assignment);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return assignments;
        }

        public void AddAssignment(AssignmentModel aa)
        {
            try
            {
                using (var con = new NpgsqlConnection(_conn))
                {
                    con.Open();
                    using (var cmd = new NpgsqlCommand(@"INSERT INTO public.t_assignment(title, start, ""end"", ""percentComplete"") VALUES ( @title, @start, @end, @percentComplete)", con))
                    {
                        cmd.Parameters.AddWithValue("@title", aa.title);
                        cmd.Parameters.AddWithValue("@start", aa.start);
                        cmd.Parameters.AddWithValue("@end", aa.end);
                        cmd.Parameters.AddWithValue("@percentComplete", aa.percentComplete * 100);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public void UpdateTask(AssignmentModel assignment)
        {
            try
            {
                using (var con = new NpgsqlConnection(_conn))
                {
                    con.Open();
                    using (var cmd = new NpgsqlCommand(@"UPDATE public.t_assignment SET title = @title, start = @start, ""end"" = @end, ""percentComplete"" = @percentComplete WHERE id = @id", con))
                    {
                        cmd.Parameters.AddWithValue("@id", assignment.id);
                        cmd.Parameters.AddWithValue("@title", assignment.title);
                        cmd.Parameters.AddWithValue("@start", assignment.start);
                        cmd.Parameters.AddWithValue("@end", assignment.end);
                        cmd.Parameters.AddWithValue("@percentComplete", assignment.percentComplete * 100);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void DeleteTask(int id)
        {
            try
            {
                using (var con = new NpgsqlConnection(_conn))
                {
                    con.Open();
                    using (var cmd = new NpgsqlCommand("DELETE FROM public.t_assignment WHERE id = @id", con))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public List<StudentModel> GetAllDobOfStudents()
        {
            List<StudentModel> students = new List<StudentModel>();
            try
            {
                conn.Open();
                string query = "SELECT c_id, c_first_name, c_dob FROM public.t_student";
                var cmd = new NpgsqlCommand(query, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        students.Add(new StudentModel
                        {
                            c_id = Convert.ToInt32(reader["c_id"]),
                            c_first_name = reader["c_first_name"].ToString(),
                            c_dob = Convert.ToDateTime(reader["c_dob"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return students;
        }

    }
}