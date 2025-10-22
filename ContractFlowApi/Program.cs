using ContractsMvc.Data;
using ContractsMvc.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ContractsDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("ContractsDb"))
       .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

builder.Services.AddScoped<ContractService>();
builder.Services.AddScoped<SupplierService>();
builder.Services.AddScoped<OrgUnitService>();
builder.Services.AddScoped<ObligationService>();
builder.Services.AddScoped<InspectionService>();
builder.Services.AddScoped<EvidenceService>();
builder.Services.AddScoped<AttachmentService>();

// Register the hosted service both as a hosted service and as a singleton
builder.Services.AddSingleton<AlertsHostedService>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<AlertsHostedService>());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ContractsDbContext>();

    db.Database.Migrate();
    await SeedData.InitializeAsync(db);
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();