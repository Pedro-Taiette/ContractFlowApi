using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Contracts.Infrastructure.Persistence;

public class DesignTimeFactory : IDesignTimeDbContextFactory<ContractsDbContext>
{
    public ContractsDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<ContractsDbContext>()
            .UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=ContractsDb;Trusted_Connection=True;MultipleActiveResultSets=True")
            .Options;
        return new ContractsDbContext(options);
    }
}
