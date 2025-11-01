using System;
using System.Collections.Generic;

namespace GraphQlDemo.Models;

public partial class User
{
    public int Id { get; set; }

    public Guid? PublicId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<AddressTable> AddressTables { get; set; } = new List<AddressTable>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
