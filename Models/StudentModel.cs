using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mvc.Models
{
    public class StudentModel
    {
        public int c_id { get; set; }
        public int c_userid { get; set; }
        public string c_first_name { get; set; }
        public string c_last_name { get; set; }
        public DateTime c_dob { get; set; }
        public string c_gender { get; set; }//
        public int c_age { get; set; }
        public string c_address { get; set; }
        public string c_contactno { get; set; }
        public string c_profile { get; set; }
        public string c_password { get; set; }
    }
}