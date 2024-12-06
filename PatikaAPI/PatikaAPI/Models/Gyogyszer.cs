using System;
using System.Collections.Generic;

namespace PatikaAPI.Models;

public partial class Gyogyszer
{
    public int Id { get; set; }

    public string Nev { get; set; } = null!;

    public string Hatoanyag { get; set; } = null!;

    public bool Venykoteles { get; set; }

    public string Kepnev { get; set; } = null!;
}
