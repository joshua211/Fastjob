﻿namespace YAJQ.Core;

public class YAJQOptions
{
    public int HandlerTimeout { get; set; } = 1000;
    public int NumberOfProcessors { get; set; } = 10;
    public int TransientFaultBaseDelay { get; set; } = 100;
    public int TransientFaultMaxTries { get; set; } = 5;
    public int MaxOverdueTimeout { get; set; } = 5;
    public int ScheduledJobTimerInterval { get; set; } = 1000;
}