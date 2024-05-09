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
        public string? c_profile { get; set; }
        public string? c_password { get; set; }

        public int GetUserIdOrDefault()
        {
            if (c_userid.HasValue)
            {
                return c_userid.Value;
            }
            else
            {
                // Return a default value when c_userid is null
                return 0;
            }
        }
    }
}