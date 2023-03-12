using System;
using System.Collections.Generic;

namespace SourceCode.Models;

public partial class AdminTbl
{
    public string Id { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public byte[]? Salt { get; set; }

    public int? NoNotifications { get; set; }

    public bool? ForgetPassword { get; set; }
}
