using System;
using System.Collections.Generic;

namespace E_Administration.Models;

public partial class Schedule
{
    public int ScheduleId { get; set; }

    public int DayId { get; set; }

    public TimeOnly ClassStartTime { get; set; }

    public TimeOnly ClassEndTime { get; set; }

    public int LabId { get; set; }

    public virtual ScheduleDay Day { get; set; } = null!;

    public virtual Lab Lab { get; set; } = null!;
}
