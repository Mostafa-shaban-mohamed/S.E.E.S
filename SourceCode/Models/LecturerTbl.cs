using System;
using System.Collections.Generic;

namespace SourceCode.Models;

public partial class LecturerTbl
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public byte[]? Salt { get; set; }

    public string Role { get; set; } = null!;

    public byte[]? Image { get; set; }

    public string? RegisteredCourses { get; set; }

    public int? NoNotification { get; set; }

    public bool? ForgetPassword { get; set; }

    public virtual ICollection<CourseTbl> CourseTbls { get; } = new List<CourseTbl>();

    public virtual ICollection<ExamTbl> ExamTbls { get; } = new List<ExamTbl>();
}
