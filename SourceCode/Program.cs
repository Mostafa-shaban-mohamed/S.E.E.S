using Microsoft.EntityFrameworkCore;
using SourceCode.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// using Microsoft.EntityFrameworkCore;
builder.Services.AddDbContext<LmsdbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMvc();
//JWT Token (For Authentication & Authorization)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(240);
});
// I am setting the default authentication and challenge scheme as JwtBearerDefaults.AuthenticationScheme.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//The AddJwtBearer will handle all requests and will check for a valid JWT Token in the header.
.AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        //inside of the code, I am setting the IssuerSigningKey using the string key “This is my test private key”
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireClaim("Role", "Admin"));
    options.AddPolicy("Student", policy => policy.RequireClaim("Role", "Student"));
    options.AddPolicy("Lecturer", policy => policy.RequireClaim("Role", "Lecturer"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
/* if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseStatusCodePagesWithRedirects("/Error/{0}");
} */

app.UseExceptionHandler("/Home/Error");
// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();

app.UseStatusCodePagesWithRedirects("/Error/{0}");
//enable session before MVC
app.UseSession();

//Authorization Middleware
// Add header:
app.Use((context, next) =>
{
    context.Request.Headers["Authorization"] = context.Session.GetString("Authorization") == null ? "" : context.Session.GetString("Authorization");
    return next.Invoke();
});

app.UseHttpsRedirection();
app.UseStaticFiles();
//enable session before MVC
app.UseSession();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
