using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PatikaAPI.Models;

public partial class Kezel
{
    public int GyogyszerId { get; set; }

    public int BetegsegId { get; set; }

    [JsonIgnore]
    public virtual Betegseg Betegseg { get; set; } = null!;

    [JsonIgnore]
    public virtual Gyogyszer Gyogyszer { get; set; } = null!;
}
