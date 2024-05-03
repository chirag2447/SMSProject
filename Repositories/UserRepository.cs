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
                cmd.Parameters.AddWithValue("@Role", userModel.c_role);
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
    }
}
