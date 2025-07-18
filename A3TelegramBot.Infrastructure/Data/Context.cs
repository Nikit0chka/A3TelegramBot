using System.Reflection;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.CallBackRequestEntity;
using A3TelegramBot.Domain.AggregateModels.UserSessionStatisticAggregates;
using Ardalis.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace A3TelegramBot.Infrastructure.Data;

/// <inheritdoc />
/// <summary>
///     Основной контекст ef core
/// </summary>
internal sealed class Context:DbContext
{
    private readonly IDomainEventDispatcher _dispatcher;

    public Context(DbContextOptions<Context> options, IDomainEventDispatcher dispatcher):base(options)
    {
        _dispatcher = dispatcher;

        Database.EnsureCreated();
    }

    public DbSet<UserSession> UserSessions { get; init; }
    public DbSet<CallBackRequestCompletedRecord> CallBackRequestCompletedRecords { get; init; }
    public DbSet<NearestReceptionSearchCompletedRecord> NearestReceptionSearchCompletedRecords { get; init; }
    public DbSet<PriceCheckRequestRecord> PriceCheckRequestRecords { get; init; }
    public DbSet<ServiceCheckRequestRecord> ServiceCheckRequestRecords { get; init; }
    public DbSet<CallBackRequest> CallBackRequests { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        var result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        // получаем сущности, с событиями
        var entitiesWithEvents = ChangeTracker.Entries<HasDomainEventsBase>()
            .Select(static entityEntry => entityEntry.Entity)
            .Where(static hasDomainEventsBase => hasDomainEventsBase.DomainEvents.Count != 0)
            .ToArray();

        // вызываем события
        await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);

        return result;
    }

    public override int SaveChanges() => SaveChangesAsync().GetAwaiter().GetResult();
}