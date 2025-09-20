using Contracts.Application;
using Contracts.Application.UseCases.CreateContract;
using Contracts.Application.UseCases.GetContractById;
using Contracts.Infrastructure;
using Contracts.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(o =>
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())); 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ContractsDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("ContractsDb"))
       .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddScoped<CreateContractHandler>();
builder.Services.AddScoped<GetContractByIdHandler>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Ready-to-go sem migrations: garante schema e seed na primeira execução
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ContractsDbContext>();
    await db.Database.MigrateAsync();
    await DbSeeder.SeedAsync(db);
}

app.UseHttpsRedirection();
app.MapControllers();

app.MapGet("/_debug/db", async (ContractsDbContext db) =>
{
    var cnn = db.Database.GetDbConnection();
    return Results.Ok(new
    {
        provider = db.Database.ProviderName,
        dataSource = cnn.DataSource,
        database = cnn.Database,
        connectionString = cnn.ConnectionString,
        contracts = await db.Contracts.CountAsync()
    });
});

app.Run();
