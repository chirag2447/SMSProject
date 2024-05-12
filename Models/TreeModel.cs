using System;

namespace SMSProject.Models
{
    public class TreeModel
    {
        public int? c_id { get; set; }
        public int? c_userid { get; set; }
        public string? c_first_name { get; set; }
        public string? c_last_name { get; set; }
        public DateTime? c_dob { get; set; }
        public string? c_gender { get; set; }
        public int? c_age { get; set; }
        public string? c_address { get; set; }
        public string? c_contact_number { get; set; }


          public int c_user_id { get; set; }
        public string c_user_first_name { get; set; }
        public string c_user_last_name { get; set; }
        public int c_user_age { get; set; }
        public string c_user_address { get; set; }
        public string c_user_gender { get; set; }
        public DateTime c_user_dob { get; set; }
        public string c_phone { get; set; }

        public int GetUserIdOrDefault()
        {
            if (c_userid.HasValue)
            {
                return c_userid.Value;
            }
            else
            {
                return 0;
            }
        }
    }
}