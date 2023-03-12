using System;
using System.Collections.Generic;

namespace SourceCode.Models;

public partial class ResultTbl
{
    public string Id { get; set; } = null!;

    public string CourseId { get; set; } = null!;

    public string StudentId { get; set; } = null!;

    public int? TotalMark { get; set; }

    public int? AchievedMark { get; set; }

    public string Title { get; set; } = null!;

    public string? ExamId { get; set; }

    public virtual CourseTbl Course { get; set; } = null!;

    public virtual ExamTbl? Exam { get; set; }

    public virtual StudentTbl Student { get; set; } = null!;

    public virtual ICollection<StudentTbl> StudentTbls { get; } = new List<StudentTbl>();
}
