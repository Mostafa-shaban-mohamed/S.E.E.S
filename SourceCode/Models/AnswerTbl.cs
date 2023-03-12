using System;
using System.Collections.Generic;

namespace SourceCode.Models;

public partial class AnswerTbl
{
    public string AnsId { get; set; } = null!;

    public string ExamId { get; set; } = null!;

    public string StuCode { get; set; } = null!;

    public string? Ans1 { get; set; }

    public string? Ans2 { get; set; }

    public string? Ans3 { get; set; }

    public string? Ans4 { get; set; }

    public string? Ans5 { get; set; }

    public string? Ans6 { get; set; }

    public string? Ans7 { get; set; }

    public string? Ans8 { get; set; }

    public string? Ans9 { get; set; }

    public string? Ans10 { get; set; }

    public int? AcheivedMark { get; set; }
}
