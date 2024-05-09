using Npgsql;
using SMSProject.Models;

namespace SMSProject.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly NpgsqlConnection conn;
        private readonly IHttpContextAccessor _accessor;
        public UserRepository(IConfiguration config, IHttpContextAccessor accessor)
        {
            conn = new NpgsqlConnection(config.GetConnectionString("SMSProject"));
            _accessor = accessor;
        }
        public void AddUser(UserModel userModel)
        {
            try
            {
                conn.Open();
                string query = "INSERT INTO public.t_user(c_first_name, c_last_name, c_gmail, c_password, c_phone, c_dob, c_gender, c_age, c_country_id, c_state_id, c_city_id, c_address, c_language, c_qualification, c_profile, c_role) VALUES (@FirstName, @LastName, @Gmail, @Password, @Phone, @DOB, @Gender, @Age, @CountryId, @StateId, @CityId, @Address, @Language, @Qualification, @Profile, @Role)";
                var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FirstName", userModel.c_first_name);
                cmd.Parameters.AddWithValue("@LastName", userModel.c_last_name);
                cmd.Parameters.AddWithValue("@Gmail", userModel.c_gmail);
                cmd.Parameters.AddWithValue("@Password", userModel.c_password);
                cmd.Parameters.AddWithValue("@Phone", userModel.c_phone);
                cmd.Parameters.AddWithValue("@DOB", userModel.c_dob);
                cmd.Parameters.AddWithValue("@Gender", userModel.c_gender);
                cmd.Parameters.AddWithValue("@Age", userModel.c_age);
                cmd.Parameters.AddWithValue("@CountryId", userModel.c_country_id);
                cmd.Parameters.AddWithValue("@StateId", userModel.c_state_id);
                cmd.Parameters.AddWithValue("@CityId", userModel.c_city_id);
                cmd.Parameters.AddWithValue("@Address", userModel.c_address);
                cmd.Parameters.AddWithValue("@Language", userModel.c_language);
                cmd.Parameters.AddWithValue("@Qualification", userModel.c_qualification);
                cmd.Parameters.AddWithValue("@Profile", userModel.c_profile);
                cmd.Parameters.AddWithValue("@Role", "User");
                cmd.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public bool IsUser(string email)
        {
            try
            {
                conn.Open();
                string query = "SELECT EXISTS(SELECT 1 FROM public.t_user WHERE c_gmail = @Gmail)";
                var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Gmail", email);
                return (bool)cmd.ExecuteScalar();
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        public bool Login(UserModel userModel)
        {
            try
            {
                conn.Open();
                string query = "SELECT c_id, c_role, c_gmail FROM public.t_user WHERE c_gmail = @Gmail AND c_password = @Password";
                var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Gmail", userModel.c_gmail);
                cmd.Parameters.AddWithValue("@Password", userModel.c_password);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        _accessor.HttpContext.Session.SetInt32("userid", Convert.ToInt32(reader["c_id"]));
                        _accessor.HttpContext.Session.SetString("role", reader["c_role"].ToString());
                        _accessor.HttpContext.Session.SetString("gmail", reader["c_gmail"].ToString());
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<CountryModel> GetCountries()
        {
            List<CountryModel> countries = new List<CountryModel>();
            try
            {
                conn.Open();
                string query = "SELECT * FROM public.t_country";
                var cmd = new NpgsqlCommand(query, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        countries.Add(new CountryModel
                        {
                            c_id = Convert.ToInt32(reader["c_id"]),
                            c_name = reader["c_name"].ToString()
                        });
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return countries;
        }

        public List<StateModel> GetStates(int countryid)
        {
            List<StateModel> states = new List<StateModel>();
            try
            {
                conn.Open();
                string query = "SELECT * FROM public.t_state WHERE c_countryid = @CountryId";
                var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CountryId", countryid);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        states.Add(new StateModel
                        {
                            c_id = Convert.ToInt32(reader["c_id"]),
                            c_name = reader["c_name"].ToString(),
                            c_countryid = Convert.ToInt32(reader["c_countryid"])
                        });
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return states;
        }

        public List<CityModel> GetCities(int stateid)
        {
            List<CityModel> cities = new List<CityModel>();
            try
            {
                conn.Open();
                string query = "SELECT * FROM public.t_city WHERE c_stateid = @StateId";
                var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@StateId", stateid);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cities.Add(new CityModel
                        {
                            c_id = Convert.ToInt32(reader["c_id"]),
                            c_name = reader["c_name"].ToString(),
                            c_stateid = Convert.ToInt32(reader["c_stateid"])
                        });
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return cities;
        }


        public UserModel GetProfile(int userid)
        {
            try
            {
                conn.Open();
                string query = "SELECT * FROM public.t_user WHERE c_id = @UserId";
                var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", userid);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new UserModel
                        {
                            c_id = Convert.ToInt32(reader["c_id"]),
                            c_first_name = reader["c_first_name"].ToString(),
                            c_last_name = reader["c_last_name"].ToString(),
                            c_gmail = reader["c_gmail"].ToString(),
                            c_password = reader["c_password"].ToString(),
                            c_phone = reader["c_phone"].ToString(),
                            c_dob = Convert.ToDateTime(reader["c_dob"]),
                            c_gender = reader["c_gender"].ToString(),
                            c_age = Convert.ToInt32(reader["c_age"]),
                            c_country_id = Convert.ToInt32(reader["c_country_id"]),
                            c_state_id = Convert.ToInt32(reader["c_state_id"]),
                            c_city_id = Convert.ToInt32(reader["c_city_id"]),
                            c_address = reader["c_address"].ToString(),
                            c_language = reader["c_language"].ToString().Split(','),
                            c_qualification = reader["c_qualification"].ToString(),
                            c_profile = reader["c_profile"].ToString(),
                            c_role = reader["c_role"].ToString()
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
