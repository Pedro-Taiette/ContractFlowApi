using System;

namespace ContractsMvc.Models;

/// <summary>
/// Base class for all entities in the simplified API. Provides common
/// properties for identity, auditing and soft deletion. All entities derive
/// from this class so that EF Core can automatically handle keys and
/// timestamps. It intentionally exposes set accessors to reduce ceremony
/// compared with the original domain model.
/// </summary>
public abstract class Entity
{
    /// <summary>
    /// Primary key for the entity. Generated when the instance is created.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Timestamp for when the entity was created. Defaults to UTC now.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Timestamp for when the entity was last updated. Nullable to allow
    /// distinguishing between never updated and updated states.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
    /// <summary>
    /// Indicates whether the entity has been soft deleted. Soft deletion
    /// allows records to be excluded from queries without physically
    /// removing them from the database.
    /// </summary>
    public bool IsDeleted { get; set; }
}

/// <summary>
/// Marker class representing an aggregate root. Currently this class does
/// not add any additional behaviour beyond Entity but is kept for
/// consistency with the domain model. Using a separate class makes it
/// obvious which entities form aggregates.
/// </summary>
public abstract class AggregateRoot : Entity
{
}