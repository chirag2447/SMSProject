using Microsoft.AspNetCore.Mvc;
using SMSProject.Models;
using SMSProject.Repository;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using mvc.Models;

namespace SMSProject.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor; // Add this

        protected readonly IWebHostEnvironment HostingEnvironment;
        protected readonly string CaptchaPath;

        public UserController(ILogger<UserController> logger, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env) // Modify this
        {
            _logger = logger;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor; // Add this

            HostingEnvironment = env;
            CaptchaPath = Path.Combine(env.WebRootPath, "content", "captcha");

            if (!Directory.Exists(CaptchaPath))
            {
                Directory.CreateDirectory(CaptchaPath);
            }
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
                if (HttpContext.Session.GetString("role") == "Teacher")
                {
                    return RedirectToAction("index", "teacher");
                }
                else if (HttpContext.Session.GetString("role") == "Admin")
                {
                    return RedirectToAction("index", "admin");
                }
                else
                {

                    ViewBag.loginsuccess = "Login Successfully";
                    return RedirectToAction("Student", "Admin");
                }
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

        [HttpGet]
        public IActionResult Reset()
        {
            CaptchaModel newCaptcha = GetCaptchaModel();
            var files = Directory.GetFiles(CaptchaPath).ToList();
            string captchaText = System.IO.File.ReadAllText(files.First(x => x.Contains($"{newCaptcha.CaptchaID}.txt")));

            HttpContext.Session.SetString("captcha" + newCaptcha.CaptchaID, captchaText);
            var captchaText1 = HttpContext.Session.GetString("captcha" + newCaptcha.CaptchaID);

            return Json(new
            {
                captchatext = captchaText,
                captcha = Url.Content("~/content/captcha/" + newCaptcha.CaptchaID + ".png"),
                captchaId = newCaptcha.CaptchaID
            });
        }

        public IActionResult AudioHandler(string captchaId)
        {
            return Content(Url.Content("~/content/captcha/" + captchaId + ".wav"));
        }

        public IActionResult Validate(string captchaId, string captcha)
        {
            captcha = captcha ?? string.Empty;

            return Json(IsCaptchaValid(captchaId, captcha));
        }

        private CaptchaModel GetCaptchaModel()
        {
            var model = new CaptchaModel();
            Random rnd = new Random();
            var files = Directory.GetFiles(CaptchaPath).ToList();
            var randomCaptchaID = Path.GetFileNameWithoutExtension(files[rnd.Next(files.Count)]);

            model.CaptchaID = randomCaptchaID;
            model.Captcha = Url.Content("~/content/captcha/" + randomCaptchaID + ".png");

            string captchaText = System.IO.File.ReadAllText(files.First(x => x.Contains($"{randomCaptchaID}.txt")));
            HttpContext.Session.SetString("captcha_" + model.CaptchaID, captchaText);

            return model;
        }

        private string GetCaptchaText(string captchaId)
        {
            string text = HttpContext.Session.GetString("captcha" + captchaId);

            return text;
        }

        private bool IsCaptchaValid(string captchaId, string captcha)
        {
            string text = GetCaptchaText(captchaId);

            return text == captcha.ToUpperInvariant();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}