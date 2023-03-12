using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SourceCode.Models;

public class LecturerController : Controller
{
    private readonly LmsdbContext _context;

    public LecturerController(LmsdbContext context)
    {
        _context = context;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult Index(string sortOrder, string searchString, string currentFilter, int? pageNumber)
    {
        //for sorting based on type
        ViewData["CurrentSort"] = sortOrder;
        ViewData["NameSortParm"] = sortOrder == "Name" ? "Name_desc" : "Name";
        ViewData["RoleSortParm"] = sortOrder == "Level" ? "Role_desc" : "Role";
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
        //fetch all teachers
        var lecs = _context.LecturerTbls.AsEnumerable();
        if (!String.IsNullOrEmpty(searchString))
        {
            lecs = lecs.Where(s => s.Name.Contains(searchString) || s.Email.Contains(searchString));
        }
        //switch cases to choose which catgorey to be ordered on
        switch (sortOrder)
        {
            case "Name_desc":
                lecs = lecs.OrderByDescending(s => s.Name);
                break;
            case "Level_desc":
                lecs = lecs.OrderByDescending(s => s.Role);
                break;
            case "Name":
                lecs = lecs.OrderBy(s => s.Name);
                break;
            case "Role":
                lecs = lecs.OrderBy(s => s.Role);
                break;
            default:
                lecs = lecs.OrderBy(s => s.Id);
                break;
        }
        //for paging
        int pageSize = 4; //no of rows in single page
        return View(PaginatedList<LecturerTbl>.Create(lecs.AsQueryable(), pageNumber ?? 1, pageSize));
    }

    //Profile Page
    [HttpGet]
    [Authorize(Roles = "Lecturer")]
    public IActionResult ProfilePage()
    {
        var lec = _context.LecturerTbls.First(m => m.Email == User.Identity.Name);
        return View(lec);
    }

    //Create New Lecturer record
    [HttpGet]
    public IActionResult AddOrEdit(string? id)
    {
        if (id != null)
        {
            var lec = _context.LecturerTbls.Find(id);
            return View(lec);
        }
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddOrEdit(LecturerTbl lec)
    {
        if (lec != null)
        {
            if (User.Identity.Name == lec.Email)
            { //edit
                _context.Update(lec);
            }
            else
            {
                //hashing the password.
                var salt = SaltGenerator.GenerateSalt();
                lec.Password = Convert.ToBase64String(SaltGenerator.ComputeHMAC_SHA256(Encoding.UTF8.GetBytes(lec.Password), salt));
                lec.Salt = salt;
                lec.ForgetPassword = false;
                //add std record to student table and save to database.
                _context.LecturerTbls.Add(lec);
            }

            _context.SaveChanges();

            return RedirectToAction("UploadImage", new { id = lec.Id });
        }

        return View(lec);
    }

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
        var lec = _context.LecturerTbls.Find(model.id);
        lec.Image = uploadedFile;
        _context.Entry(lec).State = EntityState.Modified;
        _context.SaveChanges();

        return RedirectToAction("Index", "Home");
    }

    //get answers of students on certain exam
    [HttpGet]
    [Authorize(Roles = "Lecturer")]
    public IActionResult GetAnswers(string id){
        //get answer [papers] which weren't graded yet......
        return View(_context.AnswerTbls.Where(m => m.ExamId == id && m.AcheivedMark == null).ToArray());
    }
    //grade the answer paper
    [HttpGet]
    [Authorize(Roles = "Lecturer")]
    public IActionResult GradeAnswer(string id){ //id is answer ID
        var answer = _context.AnswerTbls.Find(id);
        if(answer == null){
            return NotFound();
        }
        //get exam which should be taken
        var exam = _context.ExamTbls.Find(answer.ExamId);
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
        ViewBag.TotalMark = exam.TotalMark;
        return View(answer);
    }

    [HttpPost]
    public IActionResult GradeAnswer(AnswerTbl answer){
        if(answer != null){
            var result = new ResultTbl(){
                Id = "R" + (_context.ResultTbls.Count() + 1).ToString("0000"),
                CourseId = answer.ExamId.Substring(0,5), //this depends on my system of IDs
                StudentId = answer.StuCode,
                TotalMark = _context.ExamTbls.Find(answer.ExamId).TotalMark,
                AchievedMark = answer.AcheivedMark,
                Title = "Result of " + answer.AnsId.Substring(0,5) + " Exam", //this depends on my system of IDs
                ExamId = answer.ExamId
            };
            //chnage acheived mark in answer paper
            var ans = _context.AnswerTbls.Find(answer.AnsId);
            if(ans != null){
                ans.AcheivedMark = answer.AcheivedMark;
                _context.AnswerTbls.Update(ans);
            }
            
            _context.ResultTbls.Add(result);
            _context.SaveChanges();

            return RedirectToAction("IndexExam", "Examination");
        }
        return View(answer);
    }

}