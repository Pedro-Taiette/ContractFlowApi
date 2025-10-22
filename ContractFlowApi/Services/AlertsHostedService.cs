using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ContractsMvc.Data;
using ContractsMvc.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ContractsMvc.Services
{
    /// <summary>
    /// Background service that periodically checks for contracts and
    /// deliverables with approaching or overdue deadlines. When such
    /// conditions are detected an alert record is persisted and a log
    /// entry is produced. The service runs once daily.
    /// </summary>
    public class AlertsHostedService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<AlertsHostedService> _logger;

        public AlertsHostedService(IServiceProvider services, ILogger<AlertsHostedService> logger)
        {
            _services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Run immediately on startup
            await CheckAndGenerateAlertsAsync(stoppingToken);
            // Then run daily
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
                await CheckAndGenerateAlertsAsync(stoppingToken);
            }
        }

        /// <summary>
        /// Checks for due or upcoming items and records alerts.
        /// </summary>
        public async Task CheckAndGenerateAlertsAsync(CancellationToken ct)
        {
            using var scope = _services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ContractsDbContext>();
            var now = DateTime.UtcNow;
            var soon = now.AddDays(7);

            // Deliverables that are due soon or overdue and not delivered
            var deliverables = await db.Deliverables
                .AsNoTracking()
                .Where(d => d.DeliveredAt == null && d.ExpectedDate <= soon)
                .ToListAsync(ct);
            foreach (var d in deliverables)
            {
                var message = d.ExpectedDate < now
                    ? $"Deliverable {d.Id} is overdue (expected {d.ExpectedDate:yyyy-MM-dd})."
                    : $"Deliverable {d.Id} is due soon (expected {d.ExpectedDate:yyyy-MM-dd}).";
                await CreateAlertAsync(db, message, null, d.Id, d.ExpectedDate, ct);
                _logger.LogWarning(message);
            }

            // Contracts approaching end of term
            var contracts = await db.Contracts.AsNoTracking()
                .Where(c => c.Term.End <= soon)
                .ToListAsync(ct);
            foreach (var c in contracts)
            {
                var message = c.Term.End < now
                    ? $"Contract {c.Id} has ended on {c.Term.End:yyyy-MM-dd}."
                    : $"Contract {c.Id} is approaching end of term on {c.Term.End:yyyy-MM-dd}.";
                await CreateAlertAsync(db, message, c.Id, null, c.Term.End, ct);
                _logger.LogWarning(message);
            }
            await db.SaveChangesAsync(ct);
        }

        private static async Task CreateAlertAsync(ContractsDbContext db, string message, Guid? contractId, Guid? deliverableId, DateTime targetDate, CancellationToken ct)
        {
            var alert = new Alert
            {
                Message = message,
                ContractId = contractId,
                DeliverableId = deliverableId,
                TargetDate = targetDate
            };
            db.Alerts.Add(alert);
            // do not save here; caller will save in batch
            await Task.CompletedTask;
        }
    }
}