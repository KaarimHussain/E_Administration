using System;
using System.Collections.Generic;

namespace E_Administration.Models;

public partial class ScheduleDay
{
    public int DayId { get; set; }

    public string DayName { get; set; } = null!;

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
