using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SMSProject.Models;
using SMSProject.Repository;

namespace SMSProject.Controllers;

public class TeacherController : Controller
{
    private readonly ILogger<TeacherController> _logger;
    private readonly ITeacherRepository _teacherRepository;

    public TeacherController(ILogger<TeacherController> logger,ITeacherRepository t)
    {
        _logger = logger;
        _teacherRepository = t;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult GetStudents()
    {
        return Json(_teacherRepository.GetAllStudents());
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
