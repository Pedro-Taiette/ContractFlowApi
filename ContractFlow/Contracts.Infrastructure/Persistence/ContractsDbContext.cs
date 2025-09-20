using Contracts.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Contracts.Infrastructure.Persistence;

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
        model.ApplyConfigurationsFromAssembly(typeof(ContractsDbContext).Assembly);
    }
}
