using System;
using System.Collections.Generic;

namespace SourceCode.Models;

public partial class RegisteredCoursesTbl
{
    public string Id { get; set; } = null!;

    public string? Course01 { get; set; }

    public string? Course02 { get; set; }

    public string? Course03 { get; set; }

    public string? Course04 { get; set; }

    public string? Course05 { get; set; }

    public string? Course06 { get; set; }

    public string? Course07 { get; set; }

    public virtual CourseTbl? Course01Navigation { get; set; }

    public virtual CourseTbl? Course02Navigation { get; set; }

    public virtual CourseTbl? Course03Navigation { get; set; }

    public virtual CourseTbl? Course04Navigation { get; set; }

    public virtual CourseTbl? Course05Navigation { get; set; }

    public virtual CourseTbl? Course06Navigation { get; set; }

    public virtual CourseTbl? Course07Navigation { get; set; }

    public virtual ICollection<NotificationTbl> NotificationTbls { get; } = new List<NotificationTbl>();

    public virtual ICollection<StudentTbl> StudentTbls { get; } = new List<StudentTbl>();
}
