using System;

namespace SMSProject.Models
{
    public class UserModel
    {
        public int c_id { get; set; }
        public string c_first_name { get; set; }
        public string c_last_name { get; set; }
        public string c_gmail { get; set; }
        public string c_password { get; set; }
        public string c_phone { get; set; }
        public DateTime c_dob { get; set; }
        public string c_gender { get; set; }
        public int c_age { get; set; }
        public int c_country_id { get; set; }
        public int c_state_id { get; set; }
        public int c_city_id { get; set; }
        public string c_address { get; set; }
        public string[] c_language { get; set; }
        public string c_qualification { get; set; }
        public string c_profile { get; set; }
        public IFormFile Photo {get ; set;}
        public string c_role { get; set; }
        public string CaptchaText { get; set; }

    }
}
