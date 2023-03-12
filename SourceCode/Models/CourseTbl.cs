using System;
using System.Collections.Generic;

namespace SourceCode.Models;

public partial class CourseTbl
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Teacher { get; set; } = null!;

    public string? Pdfs { get; set; }

    public string? EdLevel { get; set; }

    public virtual ICollection<AttendanceTbl> AttendanceTbls { get; } = new List<AttendanceTbl>();

    public virtual ICollection<EventTbl> EventTbls { get; } = new List<EventTbl>();

    public virtual ICollection<ExamTbl> ExamTbls { get; } = new List<ExamTbl>();

    public virtual ICollection<FileTbl> FileTbls { get; } = new List<FileTbl>();

    public virtual ICollection<NotificationTbl> NotificationTbls { get; } = new List<NotificationTbl>();

    public virtual ICollection<QuestionTbl> QuestionTbls { get; } = new List<QuestionTbl>();

    public virtual ICollection<RegisteredCoursesTbl> RegisteredCoursesTblCourse01Navigations { get; } = new List<RegisteredCoursesTbl>();

    public virtual ICollection<RegisteredCoursesTbl> RegisteredCoursesTblCourse02Navigations { get; } = new List<RegisteredCoursesTbl>();

    public virtual ICollection<RegisteredCoursesTbl> RegisteredCoursesTblCourse03Navigations { get; } = new List<RegisteredCoursesTbl>();

    public virtual ICollection<RegisteredCoursesTbl> RegisteredCoursesTblCourse04Navigations { get; } = new List<RegisteredCoursesTbl>();

    public virtual ICollection<RegisteredCoursesTbl> RegisteredCoursesTblCourse05Navigations { get; } = new List<RegisteredCoursesTbl>();

    public virtual ICollection<RegisteredCoursesTbl> RegisteredCoursesTblCourse06Navigations { get; } = new List<RegisteredCoursesTbl>();

    public virtual ICollection<RegisteredCoursesTbl> RegisteredCoursesTblCourse07Navigations { get; } = new List<RegisteredCoursesTbl>();

    public virtual ICollection<ResultTbl> ResultTbls { get; } = new List<ResultTbl>();

    public virtual LecturerTbl TeacherNavigation { get; set; } = null!;
}
