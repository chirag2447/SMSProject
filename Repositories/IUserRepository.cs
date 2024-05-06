using Microsoft.AspNetCore.Identity;
using SMSProject.Models;

namespace SMSProject.Repository
{
    public interface IUserRepository
    {
        public void AddUser(UserModel userModel);
        public bool IsUser(string email);
        public bool Login(UserModel userModel);
        public List<CountryModel> GetCountries();
        public List<StateModel> GetStates(int countryid);
        public List<CityModel> GetCities(int stateid);
        


    }
}