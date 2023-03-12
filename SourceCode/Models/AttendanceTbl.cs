using System;
using System.Collections.Generic;

namespace SourceCode.Models;

public partial class AttendanceTbl
{
    public string Id { get; set; } = null!;

    public string StudentId { get; set; } = null!;

    public string CourseId { get; set; } = null!;

    public int NoOfAttendances { get; set; }

    public virtual CourseTbl Course { get; set; } = null!;

    public virtual StudentTbl Student { get; set; } = null!;

    public virtual ICollection<StudentTbl> StudentTbls { get; } = new List<StudentTbl>();
}
