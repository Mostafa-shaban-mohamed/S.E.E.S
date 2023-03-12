using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SourceCode.Models;

public class AdminController : Controller
{
    private readonly LmsdbContext _context;
    public AdminController(LmsdbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult Index(string searchString, string currentFilter, int? pageNumber)
    {
        //if there is a search, then its only one page
        if (searchString != null)
        {
            pageNumber = 1;
        }
        else
        {
            searchString = currentFilter;
        }
        //for searching
        ViewData["CurrentFilter"] = searchString;
        var admins = _context.AdminTbls.AsEnumerable();
        if (!String.IsNullOrEmpty(searchString))
        {
            admins = admins.Where(s => s.Email.Contains(searchString) || s.Id.Contains(searchString));
        }
        //for paging
        int pageSize = 3; //no of rows in single page
        return View(PaginatedList<AdminTbl>.Create(admins.AsQueryable(), pageNumber ?? 1, pageSize));
    }

    //Create New Admin record
    [HttpGet]
    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SignUp(AdminTbl admin)
    {
        if(admin != null){
            //hashing the password.
            var salt = SaltGenerator.GenerateSalt();
            admin.Password = Convert.ToBase64String(SaltGenerator.ComputeHMAC_SHA256(Encoding.UTF8.GetBytes(admin.Password), salt));
            admin.Salt = salt;
            admin.ForgetPassword = false;
            //add std record to student table and save to database.
            _context.AdminTbls.Add(admin);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
        return View(admin);
    }

    //Profile Page
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult ProfilePage()
    {
        var admin = _context.AdminTbls.First(m => m.Email == User.Identity.Name);
        return View(admin);
    }

    //set registered courses to right students
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult SetRegisteredCoursesStudent()
    {
        foreach(var reg in _context.RegisteredCoursesTbls.ToList())
        {
            var stds = _context.StudentTbls.Where(m => m.EdLevel == reg.Id).ToArray();
            
            foreach (var std in stds)
            {
                if(std.RegisteredCourses == null)
                    std.RegisteredCourses = reg.Id;
            }
        }
        _context.SaveChanges();
        return RedirectToAction("Index", "Student");
    }
}