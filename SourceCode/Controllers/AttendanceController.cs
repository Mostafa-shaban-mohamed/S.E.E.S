using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SourceCode.Models;

public class AttendanceController : Controller
{
    private readonly LmsdbContext _context;

    public AttendanceController(LmsdbContext context)
    {
        _context = context;
    }
    public IActionResult Index(string id)
    {
        var att = from attend in _context.AttendanceTbls.ToList()
                    join std in _context.StudentTbls.ToList() on attend.StudentId equals std.Id
                    select new AttendViewModel() 
                        { 
                          StudentName = std.Name, 
                          NoOfAttendances = attend.NoOfAttendances 
                        };

        return View(att);
    }

    //Responsible for taking attendance of students in certain course class.
    [HttpGet]
    [Authorize(Roles = "Lecturer")]
    public IActionResult CreateAttendance(string EdLevel, string courseID)
    {
        //list of students registered for this course.
        ViewBag.courseID = courseID; //course's ID
        return View(_context.StudentTbls.Where(m => m.EdLevel == EdLevel).ToArray());
    }

    [HttpPost]
    public IActionResult CreateAttendance(string[] studentsID, string courseID)
    {
        //for each student which be taken attend in this class. 
        foreach(var stdID in studentsID)
        {
            //check if this student has attendance record or not
            //if don't have one, new one will be created
            if(_context.AttendanceTbls.Count(m => m.CourseId == courseID && m.StudentId == stdID) == 0){
                var att = new AttendanceTbl(){
                    Id = "Att"+(_context.AttendanceTbls.Count()+1).ToString("0000"),
                    CourseId = courseID,
                    StudentId = stdID,
                    NoOfAttendances = 1
                };
                _context.AttendanceTbls.Add(att);
            }else{//if has, add 1 to attendance record.
                var att = _context.AttendanceTbls.First(m => m.CourseId == courseID && m.StudentId == stdID);
                att.NoOfAttendances += 1;
            }
            _context.SaveChanges();
        }   
        return RedirectToAction("Index", "Home");
    }
}