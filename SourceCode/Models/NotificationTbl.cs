using System;
using System.Collections.Generic;

namespace SourceCode.Models;

public partial class NotificationTbl
{
    public string NotificationId { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Role { get; set; } = null!;

    public bool IsNotify { get; set; }

    public string EdLevel { get; set; } = null!;

    public string CourseId { get; set; } = null!;

    public DateTime ReleaseDate { get; set; }

    public virtual CourseTbl Course { get; set; } = null!;

    public virtual RegisteredCoursesTbl EdLevelNavigation { get; set; } = null!;
}
