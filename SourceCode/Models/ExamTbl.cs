using System;
using System.Collections.Generic;

namespace SourceCode.Models;

public partial class ExamTbl
{
    public string ExamId { get; set; } = null!;

    public string CourseId { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? Q01 { get; set; }

    public string? Q02 { get; set; }

    public string? Q03 { get; set; }

    public string? Q04 { get; set; }

    public string? Q05 { get; set; }

    public string? Q06 { get; set; }

    public string? Q07 { get; set; }

    public string? Q08 { get; set; }

    public string? Q09 { get; set; }

    public string? Q10 { get; set; }

    public int? TotalMark { get; set; }

    public DateTime? ReleaseTime { get; set; }

    public DateTime? AvailabilityTime { get; set; }

    public string EdLevel { get; set; } = null!;

    public string Teacher { get; set; } = null!;

    public virtual CourseTbl Course { get; set; } = null!;

    public virtual QuestionTbl? Q01Navigation { get; set; }

    public virtual QuestionTbl? Q02Navigation { get; set; }

    public virtual QuestionTbl? Q03Navigation { get; set; }

    public virtual QuestionTbl? Q04Navigation { get; set; }

    public virtual QuestionTbl? Q05Navigation { get; set; }

    public virtual QuestionTbl? Q06Navigation { get; set; }

    public virtual QuestionTbl? Q07Navigation { get; set; }

    public virtual QuestionTbl? Q08Navigation { get; set; }

    public virtual QuestionTbl? Q09Navigation { get; set; }

    public virtual QuestionTbl? Q10Navigation { get; set; }

    public virtual ICollection<ResultTbl> ResultTbls { get; } = new List<ResultTbl>();

    public virtual LecturerTbl TeacherNavigation { get; set; } = null!;
}
