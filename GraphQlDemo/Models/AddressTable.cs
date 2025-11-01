using System;
using System.Collections.Generic;

namespace GraphQlDemo.Models;

public partial class AddressTable
{
    public int Id { get; set; }

    public int Pincode { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
