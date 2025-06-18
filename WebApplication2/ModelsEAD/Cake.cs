using System;
using System.Collections.Generic;

namespace WebApplication2.ModelsEAD;

public partial class Cake
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Price { get; set; }

    public string Description { get; set; } = null!;

    public byte[] Image { get; set; } = null!;

    public int Quantity { get; set; }
}
