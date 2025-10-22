using System;

namespace ContractsMvc.Models;

/// <summary>
/// Represents a time period with a start and end date. Used on contracts
/// to define their term. Includes a simple validation in the parameterised
/// constructor and a Contains method for convenience.
/// </summary>
public class Period
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    public Period() { }

    public Period(DateTime start, DateTime end)
    {
        if (end <= start) throw new ArgumentException("End must be after Start.");
        Start = start;
        End = end;
    }

    public bool Contains(DateTime date) => date >= Start && date <= End;
}