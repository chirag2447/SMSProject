using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSProject.Models
{
    public class CountryModel
    {
        public int c_id { get; set; }
        public string c_name { get; set; }
    }

    public class StateModel
    {
        public int c_id { get; set; }
        public string c_name { get; set; }
        public int c_countryid { get; set; }
    }

    public class CityModel
    {
        public int c_id { get; set; }
        public string c_name { get; set; }
        public int c_stateid { get; set; }
    }
}