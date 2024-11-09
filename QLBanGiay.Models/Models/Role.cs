using System;
using System.Collections.Generic;

namespace QLBanGiay.Models.Models;

public partial class Role
{
    public long Roleid { get; set; }

    public string Rolename { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
