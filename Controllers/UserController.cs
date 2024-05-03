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

        [HttpPost]
        public IActionResult Register(UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                _userRepository.AddUser(userModel);
                ViewBag.registersuccess = "Registration Successfully";
                return View();
            }
            else
            {
                ViewBag.registererror = "Registration Failed";
            }
            return View();

        }

        [HttpPost]
        public IActionResult Login(UserModel usermodel)
        {

            if (_userRepository.Login(usermodel))
            {
                ViewBag.loginsuccess = "Login Successfully";
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


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}