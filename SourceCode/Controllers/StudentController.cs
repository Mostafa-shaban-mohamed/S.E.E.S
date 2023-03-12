using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SourceCode.Models;

public class StudentController : Controller
{
    private readonly LmsdbContext _context;

    public StudentController(LmsdbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult Index()
    {
        var stds = _context.StudentTbls.ToArray();
        return View(stds);
    }

    //Profile Page
    [HttpGet]
    [Authorize(Policy = "Student")]
    public IActionResult ProfilePage()
    {
        var std = _context.StudentTbls.First(m => m.Email == User.Identity.Name);
        return View(std);
    }

    //Create New Student record
    //Or edit an existing student
    //it could be new user, so we can't use authorization attribute on this method
    //we will need to check the user in the method
    [HttpGet]
    public IActionResult AddOrEdit(string? id)
    {
        //check if user is logged in
        if (id != null && User.Identity.IsAuthenticated) //Student user
        {
            var std = _context.StudentTbls.FirstOrDefault( m => m.Id == id);
            //double check if user is the same as student data fetched
            return User.Identity.Name == std.Email? View(std) : View();
        }
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddOrEdit(StudentTbl std)
    {
        if (std != null)
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("Student"))
            { //return User (edit)
                _context.Update(std);
            }
            else //new user (create)
            {
                //hashing the password.
                var salt = SaltGenerator.GenerateSalt();
                std.Password = Convert.ToBase64String(SaltGenerator.ComputeHMAC_SHA256(Encoding.UTF8.GetBytes(std.Password), salt));
                std.Salt = salt;
                std.ForgetPassword = false;

                //add std record to student table and save to database.
                _context.StudentTbls.Add(std);
            }
            _context.SaveChanges();
            return RedirectToAction("UploadImage", new { id = std.Id });
        }
        return View(std);
    }

    //upload profile image of student account
    [HttpGet]
    public ActionResult UploadImage(string id)
    {
        var model = new FileDataViewModel() { id = id };
        return View(model);
    }

    [HttpPost]
    public ActionResult UploadImage(FileDataViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        byte[] uploadedFile = new byte[model.File.OpenReadStream().Length];
        model.File.OpenReadStream().Read(uploadedFile, 0, uploadedFile.Length);
        // now you could pass the byte array to your model and store wherever 
        // you intended to store it
        var std = _context.StudentTbls.Find(model.id);
        std.Image = uploadedFile;
        _context.Entry(std).State = EntityState.Modified;
        _context.SaveChanges();

        return RedirectToAction("Index", "Home");
    }

    //take exams
    //if available time of exam is over, then it should show error page. (later)
    [HttpGet]
    [Authorize(Roles = "Student")]
    public IActionResult TakeExam(string examId){
        //get exam which should be taken
        var exam = _context.ExamTbls.Find(examId);
        //check if valid
        if(exam == null){
            return NotFound();
        }
        //get questions in the exam
        exam.Q01Navigation = _context.QuestionTbls.First(m => m.QId == exam.Q01);
        exam.Q02Navigation = _context.QuestionTbls.First(m => m.QId == exam.Q02);
        exam.Q03Navigation = _context.QuestionTbls.First(m => m.QId == exam.Q03);
        exam.Q04Navigation = _context.QuestionTbls.First(m => m.QId == exam.Q04);
        exam.Q05Navigation = _context.QuestionTbls.First(m => m.QId == exam.Q05);
        exam.Q06Navigation = _context.QuestionTbls.FirstOrDefault(m => m.QId == exam.Q06);
        exam.Q07Navigation = _context.QuestionTbls.FirstOrDefault(m => m.QId == exam.Q07);
        exam.Q08Navigation = _context.QuestionTbls.FirstOrDefault(m => m.QId == exam.Q08);
        exam.Q09Navigation = _context.QuestionTbls.FirstOrDefault(m => m.QId == exam.Q09);
        exam.Q10Navigation = _context.QuestionTbls.FirstOrDefault(m => m.QId == exam.Q10);
        
        //pass it to the view
        ViewBag.Exam = exam;
        return View();
    }

    [HttpPost]
    public IActionResult TakeExam(AnswerTbl answer){
        if(answer != null){
            //add student id to answer paper :)
            answer.StuCode = _context.StudentTbls.First(m => m.Email == User.Identity.Name).Id;
            //add answer Id
            answer.AnsId = "Ans"+ (_context.AnswerTbls.Count() + 1).ToString() + "-" + answer.StuCode; 
            _context.AnswerTbls.Add(answer);
            _context.SaveChanges();
            return RedirectToAction("IndexExam", "Examination");
        }
        return View(answer);
    }
}