namespace ContractsMvc.Models;

/// <summary>
/// Represents the company supplying goods or services for a contract. Only
/// essential fields are retained. The Active flag can be used to disable
/// suppliers without deleting them.
/// </summary>
public class Supplier : AggregateRoot
{
    public string CorporateName { get; set; } = null!;
    public string Cnpj { get; set; } = null!;
    public bool Active { get; set; } = true;
}