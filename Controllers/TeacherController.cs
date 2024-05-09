using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SMSProject.Models;
using SMSProject.Repositories;

namespace SMSProject.Controllers
{
    // [Route("[controller]")]
    public class TeacherController : Controller
    {
        private readonly ILogger<TeacherController> _logger;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IWebHostEnvironment _webHostEnvironment;


        public TeacherController(ILogger<TeacherController> logger, ITeacherRepository teacherRepository, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _teacherRepository = teacherRepository;
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Dashboard()
        {
            TempData["Userid"] = HttpContext.Session.GetInt32("userid");
            TempData["gmail"] = HttpContext.Session.GetString("gmail");
            var data = _teacherRepository.GetAllData();
            return View(data);
        }

        public IActionResult FileManger()
        {
            TempData["gmail"] = HttpContext.Session.GetString("gmail");
            return View();
        }


        [HttpPost]
        public IActionResult UploadFile(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine("wwwroot/fileupload", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return Ok(new { fileName });
            }
            else
            {
                return BadRequest("No file uploaded.");
            }
        }
        [HttpGet]
        public IActionResult GetFiles()
        {
            try
            {
                var fileDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "fileupload");

                if (Directory.Exists(fileDirectory))
                {
                    var directoryInfo = new DirectoryInfo(fileDirectory);
                    var files = directoryInfo.GetFiles().Select(file => file.Name);
                    return Json(files);
                }
                else
                {
                    return NotFound("File directory not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}