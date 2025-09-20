namespace Contracts.Domain.Abstractions;

public abstract class Entity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; protected set; }
    public bool IsDeleted { get; protected set; }

    public void MarkUpdated() => UpdatedAt = DateTime.UtcNow;
    public void SoftDelete() { IsDeleted = true; MarkUpdated(); }
}

public abstract class AggregateRoot : Entity { }
