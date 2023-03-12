using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SourceCode.Models;

namespace SourceCode.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IConfiguration _config;
    private readonly LmsdbContext _context;

    public HomeController(ILogger<HomeController> logger, LmsdbContext context, IConfiguration config)
    {
        _logger = logger;
        _context = context;
        _config = config;
    }

    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    [HttpGet]
    public IActionResult Login()
    {
        if(User.Identity.IsAuthenticated){
            return RedirectToAction("Index");
        }
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public IActionResult Login(LoginViewModel login)
    {
        //check type of user (std/admin/lec)
        if(login.UserRole == "Student"){
            var std = _context.StudentTbls.First(m => m.Email == login.Email);
            if(std == null){
                return NotFound();
            }
            //check for password
            login.Password = Convert.ToBase64String(SaltGenerator.ComputeHMAC_SHA256(Encoding.UTF8.GetBytes(login.Password), std.Salt));
            if(std.Password != login.Password){
                return NotFound();
            }
            //token for this user
            var tokenString = GenerateJSONWebToken(login);
            HttpContext.Session.SetString("Authorization","Bearer " + tokenString);

        }else if(login.UserRole == "Admin"){
            var admin = _context.AdminTbls.First(m => m.Email == login.Email);
            if(admin == null){
                return NotFound();
            }
            //Check for password
            login.Password = Convert.ToBase64String(SaltGenerator.ComputeHMAC_SHA256(Encoding.UTF8.GetBytes(login.Password), admin.Salt));
            if(admin.Password != login.Password){
                return NotFound();
            }
            //token for this user
            var tokenString = GenerateJSONWebToken(login);
            HttpContext.Session.SetString("Authorization","Bearer " + tokenString);
            
        }else if(login.UserRole == "Lecturer"){
            var lec = _context.LecturerTbls.First(m => m.Email == login.Email);
            if(lec == null){
                return NotFound();
            }
            //Check for password
            login.Password = Convert.ToBase64String(SaltGenerator.ComputeHMAC_SHA256(Encoding.UTF8.GetBytes(login.Password), lec.Salt));
            if(lec.Password != login.Password){
                return NotFound();
            }
            //token for this user
            var tokenString = GenerateJSONWebToken(login);
            HttpContext.Session.SetString("Authorization","Bearer " + tokenString);
        }
        return RedirectToAction("Index","Home");
    }

    //Generate JSON WEB Token by passing user info
    private string GenerateJSONWebToken(LoginViewModel userInfo)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[] {    
            new Claim("Role", userInfo.UserRole),
            new Claim(ClaimTypes.Name, userInfo.Email),
            new Claim(ClaimTypes.NameIdentifier, userInfo.Email),
            new Claim(ClaimTypes.Role, userInfo.UserRole)
        }; 
        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
          _config["Jwt:Issuer"],
          claims,
          expires: DateTime.Now.AddMinutes(240),
          signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    //Logout method
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Logout()
    {
        HttpContext.Session.Remove("Authorization");
        return RedirectToAction("Index");
    }

    //Redirect Methods to correct controller
    public IActionResult RedirectProfile(){
        if(User.IsInRole("Student")) //redirect to student controller
            return RedirectToAction("ProfilePage", "Student");
        else if(User.IsInRole("Admin")) //redirect to admin controller
            return RedirectToAction("ProfilePage", "Admin");
        else //redirect to lecturer controller
            return RedirectToAction("ProfilePage", "lecturer");
    }
}
