using System;
using System.Collections.Generic;

namespace SourceCode.Models;

public partial class StudentTbl
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public byte[]? Salt { get; set; }

    public string Class { get; set; } = null!;

    public string EdLevel { get; set; } = null!;

    public byte[]? Image { get; set; }

    public string? AttendCourses { get; set; }

    public string? RegisteredCourses { get; set; }

    public string? Results { get; set; }

    public bool ForgetPassword { get; set; }

    public int? NoNotifications { get; set; }

    public virtual AttendanceTbl? AttendCoursesNavigation { get; set; }

    public virtual ICollection<AttendanceTbl> AttendanceTbls { get; } = new List<AttendanceTbl>();

    public virtual RegisteredCoursesTbl? RegisteredCoursesNavigation { get; set; }

    public virtual ICollection<ResultTbl> ResultTbls { get; } = new List<ResultTbl>();

    public virtual ResultTbl? ResultsNavigation { get; set; }
}
