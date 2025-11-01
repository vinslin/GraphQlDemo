using System;
using System.Collections.Generic;

namespace GraphQlDemo.Models;

public partial class Employee
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Phone { get; set; }

    public decimal Salary { get; set; }

    public string Email { get; set; } = null!;
}
