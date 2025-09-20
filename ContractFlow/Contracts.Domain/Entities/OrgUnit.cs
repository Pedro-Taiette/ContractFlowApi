using Contracts.Domain.Abstractions;

namespace Contracts.Domain.Entities;
public class OrgUnit : AggregateRoot
{
    public string Name { get; private set; } = null!;
    public string? Code { get; private set; }

    private OrgUnit() { }
    public OrgUnit(string name, string? code = null)
    {
        Name = name;
        Code = code;
    }
}
