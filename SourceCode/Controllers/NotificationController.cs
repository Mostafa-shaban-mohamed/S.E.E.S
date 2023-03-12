using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SourceCode.Models;

public class NotificationController : Controller
{
    private readonly LmsdbContext _context;

    public NotificationController(LmsdbContext context)
    {
        _context = context;
    }

    //it depends on user role (std, lec, admin)
    //for std notification on: new material, new exam, new assigment (event)
    //for lec notification on: new material of his/her courses, new exam of his/her courses,
    //                         new assigment (event) of his/her courses
    //for admin notification on: .......
    [HttpGet]
    [Authorize(Roles = "Lecturer, Admin, Student")]
    public IActionResult NotifyList()
    {
        if(User.IsInRole("Lecturer")){
            var lec = _context.LecturerTbls.First(m => m.Email == User.Identity.Name);
            var notList = new List<NotificationTbl>();
            var courses = _context.CourseTbls.Where(m => m.Teacher == lec.Id).ToList();
            foreach (var course in courses)
            {
                notList.AddRange(_context.NotificationTbls.Where(m => m.IsNotify == false && m.CourseId == course.Id).AsEnumerable());
            }
            return View(notList.OrderByDescending(m => m.ReleaseDate).ToArray());    
        }else if(User.IsInRole("Student")){
            var std = _context.StudentTbls.First(m => m.Email == User.Identity.Name);
            return View(_context.NotificationTbls.Where(m => m.IsNotify == false && m.EdLevel == std.EdLevel).OrderByDescending(m => m.ReleaseDate).ToArray());
        }
        return View(_context.NotificationTbls.Where(m => m.IsNotify == false).OrderByDescending(m => m.ReleaseDate).ToArray());
    }

    //New File Added
    public void CreateNotification(FileTbl fileTbl){
        var not = new NotificationTbl(){
            NotificationId = "Not" + fileTbl.Id.ToString() + "-" + "file",
            Description = "New File of " + _context.CourseTbls.Find(fileTbl.CourseId).Name,
            Role = "Lecturer",
            IsNotify = false,
            EdLevel = _context.CourseTbls.FirstOrDefault(m => m.Id == fileTbl.CourseId).EdLevel,
            CourseId = fileTbl.CourseId,
            ReleaseDate = DateTime.Now
        };
        _context.NotificationTbls.Add(not);
        _context.SaveChanges();
    }

    //New Exam Added
    public void CreateNotification(ExamTbl examTbl){
        var not = new NotificationTbl(){
            NotificationId = "Not" + examTbl.ExamId + "-" + "Exam",
            Description = "New " + examTbl.Type + " of " + _context.CourseTbls.Find(examTbl.CourseId).Name,
            Role = "Lecturer",
            IsNotify = false,
            EdLevel = examTbl.EdLevel,
            CourseId = examTbl.CourseId,
            ReleaseDate = DateTime.Now
        };
        _context.NotificationTbls.Add(not);
        _context.SaveChanges();
    }

    //New Event Added
    public void CreateNotification(EventTbl eventTbl){
        var not = new NotificationTbl(){
            NotificationId = "Not" + eventTbl.Id + "-" + "Event",
            Description = "New " + eventTbl.Type + " of " + _context.CourseTbls.Find(eventTbl.CourseId).Name,
            Role = User.IsInRole("Lecturer") == true ? "Lecturer" : "Admin",
            IsNotify = false,
            EdLevel = eventTbl.EdLevel,
            CourseId = eventTbl.CourseId,
            ReleaseDate = DateTime.Now
        };
        _context.NotificationTbls.Add(not);
        _context.SaveChanges();
    }

    //delete notifications after 14 days
    public void DeleteNotification(){
        var nots = _context.NotificationTbls.Where(m => m.ReleaseDate > m.ReleaseDate.AddDays(14.0d));
        _context.RemoveRange(nots);
        _context.SaveChanges();
    }
}