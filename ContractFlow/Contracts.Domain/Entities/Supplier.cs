using Contracts.Domain.Abstractions;

namespace Contracts.Domain.Entities;
public class Supplier : AggregateRoot
{
    public string CorporateName { get; private set; } = null!;
    public string Cnpj { get; private set; } = null!;
    public bool Active { get; private set; } = true;

    private Supplier() { }
    public Supplier(string corporateName, string cnpj)
    {
        CorporateName = corporateName;
        Cnpj = cnpj;
    }
}
