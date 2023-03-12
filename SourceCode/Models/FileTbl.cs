using System;
using System.Collections.Generic;

namespace SourceCode.Models;

public partial class FileTbl
{
    public int Id { get; set; }

    public string FileName { get; set; } = null!;

    public DateTime? UploadOn { get; set; }

    public byte[] File { get; set; } = null!;

    public string CourseId { get; set; } = null!;

    public string? AssignmentId { get; set; }

    public virtual CourseTbl Course { get; set; } = null!;
}
