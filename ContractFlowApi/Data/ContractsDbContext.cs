using ContractsMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace ContractsMvc.Data;

/// <summary>
/// DbContext for the simplified Contract Management API. It exposes DbSet
/// properties for each entity and applies entity configurations found in the
/// assembly.
/// </summary>
public class ContractsDbContext : DbContext
{
    public ContractsDbContext(DbContextOptions<ContractsDbContext> options) : base(options) { }

    public DbSet<Contract> Contracts => Set<Contract>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<OrgUnit> OrgUnits => Set<OrgUnit>();
    public DbSet<Obligation> Obligations => Set<Obligation>();
    public DbSet<Deliverable> Deliverables => Set<Deliverable>();
    public DbSet<Inspection> Inspections => Set<Inspection>();
    public DbSet<Evidence> Evidences => Set<Evidence>();
    public DbSet<NonCompliance> NonCompliances => Set<NonCompliance>();
    public DbSet<Penalty> Penalties => Set<Penalty>();
    public DbSet<Attachment> Attachments => Set<Attachment>();

    protected override void OnModelCreating(ModelBuilder model)
    {
        // Discover and apply configurations defined in this assembly
        model.ApplyConfigurationsFromAssembly(typeof(ContractsDbContext).Assembly);
    }
}