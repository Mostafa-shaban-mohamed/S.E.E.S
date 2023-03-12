using System;
using System.Collections.Generic;

namespace SourceCode.Models;

public partial class QuestionTbl
{
    public string QId { get; set; } = null!;

    public string? Ch01 { get; set; }

    public string? Ch02 { get; set; }

    public string? Ch03 { get; set; }

    public string? Ch04 { get; set; }

    public string? CorrectCh { get; set; }

    public int? TotalMark { get; set; }

    public string? QuesTitle { get; set; }

    public string? CourseId { get; set; }

    public string? QuesType { get; set; }

    public virtual CourseTbl? Course { get; set; }

    public virtual ICollection<ExamTbl> ExamTblQ01Navigations { get; } = new List<ExamTbl>();

    public virtual ICollection<ExamTbl> ExamTblQ02Navigations { get; } = new List<ExamTbl>();

    public virtual ICollection<ExamTbl> ExamTblQ03Navigations { get; } = new List<ExamTbl>();

    public virtual ICollection<ExamTbl> ExamTblQ04Navigations { get; } = new List<ExamTbl>();

    public virtual ICollection<ExamTbl> ExamTblQ05Navigations { get; } = new List<ExamTbl>();

    public virtual ICollection<ExamTbl> ExamTblQ06Navigations { get; } = new List<ExamTbl>();

    public virtual ICollection<ExamTbl> ExamTblQ07Navigations { get; } = new List<ExamTbl>();

    public virtual ICollection<ExamTbl> ExamTblQ08Navigations { get; } = new List<ExamTbl>();

    public virtual ICollection<ExamTbl> ExamTblQ09Navigations { get; } = new List<ExamTbl>();

    public virtual ICollection<ExamTbl> ExamTblQ10Navigations { get; } = new List<ExamTbl>();
}
