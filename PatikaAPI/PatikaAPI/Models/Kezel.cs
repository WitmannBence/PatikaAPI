using System;
using System.Collections.Generic;

namespace PatikaAPI.Models;

public partial class Kezel
{
    public int GyogyszerId { get; set; }

    public int BetegsegId { get; set; }

    public int Id { get; set; }

    public virtual Betegseg Betegseg { get; set; } = null!;

    public virtual Gyogyszer Gyogyszer { get; set; } = null!;
}
