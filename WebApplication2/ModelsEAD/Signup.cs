using System;
using System.Collections.Generic;

namespace WebApplication2.ModelsEAD;

public partial class Signup
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;
}
