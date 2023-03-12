using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SourceCode.Models;

public class ExaminationController : Controller
{
    private readonly LmsdbContext _context;

    public ExaminationController(LmsdbContext context)
    {
        _context = context;
    }

    //Question Part -----------------------------------------------------------

    [HttpGet]
    [Authorize(Roles = "Lecturer")]
    public IActionResult Index()
    {
        return View(_context.QuestionTbls.ToArray());
    }

    [HttpGet]
    [Authorize(Roles = "Lecturer")]
    public IActionResult AddOrEditQuestion(string? id)
    {
        //add only courses which User (Lecturer) is assigned to.
        var lec = _context.LecturerTbls.First(m => m.Email == User.Identity.Name);
        var courses = _context.CourseTbls.AsEnumerable();
        if (lec != null)
        {
            courses = courses.Where(m => m.Teacher == lec.Id);
        }
        //assign the courses list as required
        ViewBag.Courses = new SelectList(courses.ToArray(), "Id", "Name");
        //Edit Question
        if (id != null)
        {
            var question = _context.QuestionTbls.Find(id);
            return View(question);
        }
        return View(); //Add Question
    }

    [HttpPost]
    public IActionResult AddOrEditQuestion(QuestionTbl question)
    {
        if (question != null)
        {
            if (question.QId == null)
            { //create
                question.QId = "Ques" + (_context.QuestionTbls.Count() + 1).ToString("0000");
                _context.QuestionTbls.Add(question);
            }
            else
            { //edit 
                if (question.QuesType == "Written")
                {
                    question.Ch01 = null; question.Ch02 = null; question.Ch03 = null; question.Ch04 = null;
                    question.CorrectCh = null;
                }
                _context.QuestionTbls.Update(question);
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(question);
    }

    [HttpGet]
    public IActionResult DeleteQuestion(string id)
    {
        _context.QuestionTbls.Remove(_context.QuestionTbls.Find(id));
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    //Quiz Part -----------------------------------------------

    //fetch all exams based on user role (for all users)
    [HttpGet]
    [Authorize(Roles = "Student, Lecturer, Admin")]
    public IActionResult IndexExam()
    {
        if (User.IsInRole("Student"))
        {//if user is student
            var std = _context.StudentTbls.First(m => m.Email == User.Identity.Name);
            if (std == null)
            {
                return NoContent();
            }
            //get all exams that this student should have.
            var exams = _context.ExamTbls.Where(m => m.EdLevel == std.EdLevel).ToList();
            //then check if this student has answered any of those exams
            foreach (var exam in _context.ExamTbls.Where(m => m.EdLevel == std.EdLevel).ToList())
            {
                if (_context.AnswerTbls.Count(m => m.StuCode == std.Id && m.ExamId == exam.ExamId) > 0)
                {
                    exams.Remove(exam);
                }
            }
            return View(exams);
        }
        else if (User.IsInRole("Lecturer"))
        {//if user is teacher
            var lec = _context.LecturerTbls.First(m => m.Email == User.Identity.Name);
            return View(_context.ExamTbls.Where(m => m.Teacher == lec.Id).ToArray());
        }
        return View(_context.ExamTbls.ToArray());
    }

    //Create New Exam or Edit an Exam
    [HttpGet]
    [Authorize(Roles = "Lecturer")]
    public IActionResult CreateOrEditExam(string? id)
    {
        var lec = _context.LecturerTbls.First(m => m.Email == User.Identity.Name);
        ViewBag.Courses = new SelectList(_context.CourseTbls.Where(m => m.Teacher == lec.Id).ToArray(), "Id", "Name");
        //should have filteration on questions of certain course........!!!!!
        ViewBag.Questions = new SelectList(_context.QuestionTbls.ToArray(), "QId", "QuesTitle");
        //edit ot create condition
        if (id != null)
        {
            return View(_context.ExamTbls.First(m => m.ExamId == id));
        }
        return View();
    }

    [HttpPost]
    public IActionResult CreateOrEditExam(ExamTbl exam)
    {
        if (exam != null)
        {
            //add questions object to navigators
            exam.Q01Navigation = _context.QuestionTbls.Find(exam.Q01); //we can also use First(m => m.QId == exam.Q0X)
            exam.Q02Navigation = _context.QuestionTbls.Find(exam.Q02);
            exam.Q03Navigation = _context.QuestionTbls.Find(exam.Q03);
            exam.Q04Navigation = _context.QuestionTbls.Find(exam.Q04);
            exam.Q05Navigation = _context.QuestionTbls.Find(exam.Q05);

            //get total mark
            exam.TotalMark = (exam.Q01Navigation.TotalMark + exam.Q02Navigation.TotalMark + exam.Q03Navigation.TotalMark
                + exam.Q04Navigation.TotalMark + exam.Q05Navigation.TotalMark);

            if (exam.Type == "Quiz")
            {
                exam.ReleaseTime = exam.AvailabilityTime.Value.AddMinutes(30);
                exam.Q06 = null; exam.Q07 = null; exam.Q08 = null;
                exam.Q09 = null; exam.Q10 = null;
            }
            else
            {
                exam.ReleaseTime = exam.AvailabilityTime.Value.AddMinutes(60);
                //add questions ovject to navigators
                exam.Q06Navigation = _context.QuestionTbls.Find(exam.Q06);
                exam.Q07Navigation = _context.QuestionTbls.Find(exam.Q07);
                exam.Q08Navigation = _context.QuestionTbls.Find(exam.Q08);
                exam.Q09Navigation = _context.QuestionTbls.Find(exam.Q09);
                exam.Q10Navigation = _context.QuestionTbls.Find(exam.Q10);

                //get total mark
                exam.TotalMark += (exam.Q06Navigation.TotalMark + exam.Q07Navigation.TotalMark
                + exam.Q08Navigation.TotalMark + exam.Q09Navigation.TotalMark + exam.Q10Navigation.TotalMark);
            }
            //assign teacher ID [from user identity] to exam
            exam.Teacher = _context.LecturerTbls.First(m => m.Email == User.Identity.Name).Id;

            //new exam
            if (exam.ExamId == null)
            {
                //create exam id
                exam.ExamId = exam.CourseId + "EX" + (_context.ExamTbls.Count() + 1).ToString("000");
                //add teacher ID
                exam.Teacher = _context.LecturerTbls.First(m => m.Email == User.Identity.Name).Id;

                _context.ExamTbls.Add(exam);
                _context.SaveChanges();

                //add notification
                NotificationController notificationController = new NotificationController(_context);
                notificationController.CreateNotification(exam);

                return RedirectToAction("IndexExam");
            }
            else
            {//edit exam
                _context.ExamTbls.Update(exam);
                _context.SaveChanges();
                return RedirectToAction("IndexExam");
            }
        }
        return View(exam);
    }
}