namespace ContractsMvc.Models;

/// <summary>
/// Represents an organisational unit responsible for one or more contracts.
/// The code is optional and may be used to map internal identifiers. Only
/// basic properties are kept to simplify the model for the MVP.
/// </summary>
public class OrgUnit : AggregateRoot
{
    public string Name { get; set; } = null!;
    public string? Code { get; set; }
}