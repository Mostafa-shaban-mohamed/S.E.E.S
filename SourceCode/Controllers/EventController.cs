using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SourceCode.Models;

public class EventController : Controller
{
    private readonly LmsdbContext _context;

    public EventController(LmsdbContext context)
    {
        _context = context;
    }
    //authorized for lecturers to assign assigments or events
    //authorized for admins to assign events only
    [HttpGet]
    [Authorize(Roles = "Student, Admin, Lecturer")]
    public IActionResult Index()
    {
        return View(_context.EventTbls.ToArray());
    }
    //Add or Edit new event
    [HttpGet]
    [Authorize(Roles = "Admin, Lecturer")]
    public IActionResult AddOrEditEvent(string? id){

        //Check if user is lecturer or admin
        if(User.IsInRole("Lecturer")){
            var lec = _context.LecturerTbls.First(m => m.Email == User.Identity.Name);
            //get all courses that this lecturer
            ViewBag.Courses = new SelectList(_context.CourseTbls.Where(m => m.Teacher == lec.Id).ToArray(), "Id", "Name");
        }else{
            //get all courses registered in system
            ViewBag.Courses = new SelectList(_context.CourseTbls.ToArray(), "Id", "Name");
        }
        //check if add or edit
        if(id != null){
            var eventTbl = _context.EventTbls.Find(id);
            return eventTbl == null ? NotFound() : View(eventTbl);
        }
        return View();
    }
    [HttpPost]
    public IActionResult AddOrEditEvent(EventTbl eventTbl){
        if(eventTbl != null){
            if(eventTbl.Id == null){ //new event
                eventTbl.Id = "Ev" + (_context.EventTbls.Count() + 1).ToString("0000");
                eventTbl.ReleaseDate = DateTime.Now;
                _context.EventTbls.Add(eventTbl);
                
                //add notification
                NotificationController notificationController = new NotificationController(_context);
                notificationController.CreateNotification(eventTbl);
            }else{ //edit event
                _context.EventTbls.Update(eventTbl);
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View();
    }
    //delete event
    [HttpGet]
    public IActionResult DeleteEvent(string id){
        var eventTbl = _context.EventTbls.Find(id);
        if(eventTbl != null){
            _context.EventTbls.Remove(eventTbl);
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }
}