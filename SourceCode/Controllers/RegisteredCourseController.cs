using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SourceCode.Models;

public class RegisteredCourseController : Controller
{
    private readonly LmsdbContext _context;

    public RegisteredCourseController(LmsdbContext context)
    {
        _context = context;
    }

    //User is Student
    [HttpGet]
    [Authorize(Roles = "Student")]
    public IActionResult UserCourses(string EdLevel)
    {
        if(EdLevel != null){
            return View(_context.RegisteredCoursesTbls.First(m => m.Id == EdLevel));
        }
        return NotFound();
    }
/* 
    [HttpGet]
    public IActionResult Index()
    {
        return View(_context.RegisteredCoursesTbls.ToArray());
    } */

    [HttpGet]
    public IActionResult AddOrEditRegisteration(string? id)
    {
        //prepeare courses list
        ViewBag.Courses = new SelectList(_context.CourseTbls.ToArray(), "Id", "Name");

        if(id != null){
            return View(_context.RegisteredCoursesTbls.Find(id));
        }
        
        return View();
    }

    [HttpPost]
    public IActionResult AddOrEditRegisteration(RegisteredCoursesTbl reg)
    {
        if(reg != null) {
            if(User.Identity.IsAuthenticated && User.IsInRole("Admin")){
                _context.Update(reg);
            }else{
                _context.RegisteredCoursesTbls.Add(reg);
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        return View(reg);
    }
}