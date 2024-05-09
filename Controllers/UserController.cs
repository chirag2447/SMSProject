using Microsoft.AspNetCore.Mvc;
using SMSProject.Models;
using SMSProject.Repository;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace SMSProject.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor; // Add this

        public UserController(ILogger<UserController> logger, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor) // Modify this
        {
            _logger = logger;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor; // Add this
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        public static string profile = "";

        [HttpPost]
        public IActionResult Register(UserModel userModel)
        {
                userModel.c_profile = profile;
                _userRepository.AddUser(userModel);
                ViewBag.registersuccess = "Registration Successfully";
                return View();
            
           
            
         
        }

        [HttpPost]
        public IActionResult UploadPhoto(IFormFile Photo)
        {

            var file = Guid.NewGuid().ToString() + Path.GetExtension(Photo.FileName);
            var path = Path.Combine("wwwroot/images", file);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                Photo.CopyTo(stream);
            }
            profile = file;
            return Ok(new { fileName = file });
        }

        [HttpPost]
        public IActionResult Login(UserModel usermodel)
        {

            if (_userRepository.Login(usermodel))
            {
                TempData["loginsuccess"] = "Login Successfully";
                return View();
            }
            else
            {
                ViewBag.loginerror = "Not a valid Credential";
            }
            return View();

        }

        [HttpGet]
        public IActionResult GenerateCaptcha()
        {
            var captchaText = new string(Enumerable.Repeat("abcdefghijklmnopqrstuvwxyz0123456789", 5)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());

            HttpContext.Session.SetString("Captcha", captchaText);

            return Ok(captchaText);
        }

        [HttpPost]
        public IActionResult Submit(UserModel model)
        {
            var captchaText = HttpContext.Session.GetString("Captcha");
            if (model.CaptchaText != captchaText)
            {
                ModelState.AddModelError("CaptchaText", "Invalid captcha.");
                return View(model);
            }
            return RedirectToAction("Success");
        }

        public IActionResult GetAllCountries()
        {
            var countries = _userRepository.GetCountries();
            return Json(countries);
        }

        public IActionResult GetStates(int countryid)
        {
            var states = _userRepository.GetStates(countryid);
            return Json(states);
        }

        public IActionResult GetCities(int stateid)
        {
            var cities = _userRepository.GetCities(stateid);
            return Json(cities);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}