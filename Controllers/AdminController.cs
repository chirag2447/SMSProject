using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mvc.Models;
using SMSProject.Repository;

namespace SMSProject.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;

        private readonly IAdminRepository _adminRepository;
        private IWebHostEnvironment _webHostEnvironment;

        public AdminController(ILogger<AdminController> logger, IWebHostEnvironment webHostEnvironment, IAdminRepository studentRepository)
        {
            _webHostEnvironment = webHostEnvironment;
            _adminRepository = studentRepository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult PdfViewer()
        {
            return View();
        }
        public IActionResult Student()
        {
            TempData["Userid"] = HttpContext.Session.GetInt32("userid");
            return View();
        }

        [HttpPost]
        public IActionResult UploadPhoto(IFormFile? photo)
        {
            try
            {
                if (photo != null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
                    string filepath = Path.Combine(_webHostEnvironment.WebRootPath, "photos", filename);

                    using (var stream = new FileStream(filepath, FileMode.Create))
                    {
                        photo.CopyTo(stream);
                    }

                    var studModel = new StudentModel { c_profile = filename };
                    _adminRepository.Insert(studModel);

                    return Json(new { success = true, filename });
                }
                else
                {
                    return Json(new { success = false, message = "No file uploaded" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Student(StudentModel studModel)
        {
            _adminRepository.Insert(studModel);
            return Json(new { success = true });
        }

        public IActionResult ViewDatasearch(string query)
        {
            var students = _adminRepository.SearchStudents(query);
            return Json(students);
        }

        public ActionResult Pagination(int pageNumber, int pageSize)
        {
            var dökData = _adminRepository.GetDataPagination(pageNumber, pageSize);
            return Json(dökData);
        }

        public IActionResult Delete(int id)
        {
            _adminRepository.Delete(id);
            return Json(new { success = true });
        }
        public IActionResult ViewData()
        {
            var datas = _adminRepository.GetAllData();
            return Json(datas);
        }
        public IActionResult ViewTeacherData()
        {
            var datas = _adminRepository.GetAllTeacherData();
            return Json(datas);
        }

        [HttpPost]
        public IActionResult Multidelete([FromBody] List<int> ids)
        {
            if (ids != null && ids.Count > 0)
            {
                _adminRepository.multidelete(ids);
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult update(StudentModel studentModel)
        {
            _adminRepository.Update(studentModel);
            return Json(new { success = true });
        }

        public IActionResult TreeList()
        {
            return View();
        }

        public IActionResult GetStudents()
        {
            return Json(_adminRepository.GetAllStudents());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

        [HttpGet]
        public IActionResult GetStudentDobs()
        {
            // Replace with your actual repository instance
            var students = _adminRepository.GetAllDobOfStudents();

            var events = students.Select(s => new
            {
                title = s.c_first_name,
                start = s.c_dob,
                end = s.c_dob, // You might want to calculate a different end date
                id = s.c_id
            });

            return Json(events);
        }


    }
}