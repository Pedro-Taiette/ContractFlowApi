namespace ContractsMvc.Models;

/// <summary>
/// Represents the various lifecycle states a contract may occupy. The
/// underlying values match the original domain model and the status will be
/// stored as a string in the database through configuration.
/// </summary>
public enum ContractStatus
{
    Draft = 1,
    Active = 2,
    Suspended = 3,
    Closed = 4,
    Terminated = 5,
    Cancelled = 6
}