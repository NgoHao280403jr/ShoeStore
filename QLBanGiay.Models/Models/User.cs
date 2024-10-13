using System;
using System.Collections.Generic;

namespace QLBanGiay.Models.Models;

public partial class User
{
    public long Userid { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public long Roleid { get; set; }

    public bool Isactive { get; set; }

    public bool Isbanned { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual Role Role { get; set; } = null!;
}
