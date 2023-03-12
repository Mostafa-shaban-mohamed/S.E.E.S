using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MimeMapping;
using SourceCode.Models;

public class CourseController : Controller
{
    private readonly LmsdbContext _context;

    public CourseController(LmsdbContext context)
    {
        _context = context;
    }

    [Authorize(Roles = "Admin, Lecturer")]
    public IActionResult Index(string sortOrder, string searchString, string currentFilter, int? pageNumber, string? id)
    {
        //for sorting based on type
        ViewData["CurrentSort"] = sortOrder;
        ViewData["IDSortParm"] = String.IsNullOrEmpty(sortOrder) ? "ID_desc" : "";
        ViewData["NameSortParm"] = sortOrder == "Name" ? "Name_desc" : "Name";
        ViewData["LvlSortParm"] = sortOrder == "Level" ? "Lvl_desc" : "Level";
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
        //if user is admin, fetch all courses
        var courses = _context.CourseTbls.AsEnumerable();
        //if user is teacher, fetch all courses that he/she teaches only
        if(User.IsInRole("Lecturer")){
            courses = courses.Where(m => m.Teacher == id).AsEnumerable();
        }
        
        if (!String.IsNullOrEmpty(searchString))
        {
            courses = courses.Where(s => s.Name.Contains(searchString) || s.EdLevel.Contains(searchString));
        }
        //switch cases to choose which catgorey to be ordered on
        switch (sortOrder)
        {
            case "ID_desc":
                courses = courses.OrderByDescending(s => s.Id);
                break;
            case "Name_desc":
                courses = courses.OrderByDescending(s => s.Name);
                break;
            case "Level_desc":
                courses = courses.OrderByDescending(s => s.EdLevel);
                break;
            case "Name":
                courses = courses.OrderBy(s => s.Name);
                break;
            case "Level":
                courses = courses.OrderBy(s => s.EdLevel);
                break;
            default:
                courses = courses.OrderBy(s => s.Id);
                break;
        }
        //for paging
        int pageSize = 5; //no of rows in single page
        return View(PaginatedList<CourseTbl>.Create(courses.AsQueryable(), pageNumber ?? 1, pageSize));
    }

    //Details of course
    [HttpGet]
    [Authorize(Roles = "Admin, Lecturer, Student")]
    public IActionResult CourseDetails(string id) //id is course ID
    {
        if (id != null)
        {
            var course = _context.CourseTbls.First(m => m.Id == id);
            return View(course);
        }
        return NotFound();
    }

    //Upload PDFs and material files to Course ----------------------------
    [HttpGet]
    [Authorize(Roles = "Lecturer")]
    public ActionResult UploadFiles(string id)
    {
        var model = new FileDataViewModel() { id = id };
        return View(model);
    }

    [HttpPost]
    public ActionResult UploadFiles(FileDataViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        byte[] uploadedFile = new byte[model.File.OpenReadStream().Length];
        model.File.OpenReadStream().Read(uploadedFile, 0, uploadedFile.Length);

        // now you could pass the byte array to your model and store wherever 
        // you intended to store it
        FileTbl fl = new FileTbl()
        {
            FileName = model.File.FileName,
            UploadOn = DateTime.Now,
            File = uploadedFile,
            CourseId = model.id
        };
        _context.FileTbls.Add(fl);
        _context.SaveChanges(); //so when we check no of files that has course id we will find the one recently saved

        _context.CourseTbls.Find(model.id).Pdfs = _context.FileTbls.Where(m => m.CourseId == model.id).Count().ToString();
        _context.SaveChanges();

        // add notification for every student registered to this course
        NotificationController notificationController = new NotificationController(_context);
        notificationController.CreateNotification(fl);

        return RedirectToAction("CourseDetails", "Course", new { id = model.id });
    }

    //display all files for certain course
    [HttpGet]
    [Authorize(Roles = "Lecturer, Student")]
    public IActionResult CourseFiles(string id)
    {
        ViewData["CourseID"] = id;
        return View(_context.FileTbls.Where(m => m.CourseId == id).ToArray());
    }

    //View and Download PDFs and material files of this course
    public FileResult ViewFile(string Name)
    {
        var fl = _context.FileTbls.First(m => m.FileName == Name);
        string contentType = MimeUtility.GetMimeMapping(fl.FileName);
        return File(fl.File, contentType);
    }

    //Upload PDFs and material files to Course -------------------------------------
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult CreateCourse()
    {
        ViewBag.TeacherList = new SelectList(_context.LecturerTbls.ToArray(), "Id", "Name");
        return View();
    }
    [HttpPost]
    public IActionResult CreateCourse(CourseTbl course)
    {
        if (course != null)
        {

            _context.CourseTbls.Add(course);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        return View();
    }
}