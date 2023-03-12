using System;
using System.Collections.Generic;

namespace SourceCode.Models;

public partial class EventTbl
{
    public string Id { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime ReleaseDate { get; set; }

    public string Type { get; set; } = null!;

    public string? EdLevel { get; set; }

    public string? CourseId { get; set; }

    public virtual CourseTbl? Course { get; set; }
}
