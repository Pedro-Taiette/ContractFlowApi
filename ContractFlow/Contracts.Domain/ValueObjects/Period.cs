namespace Contracts.Domain.ValueObjects;

public class Period
{
    public DateTime Start { get; private set; }
    public DateTime End { get; private set; }

    private Period() { } // EF

    public Period(DateTime start, DateTime end)
    {
        if (end <= start) throw new ArgumentException("End must be after Start.");
        Start = start;
        End = end;
    }

    public bool Contains(DateTime date) => date >= Start && date <= End;
}
