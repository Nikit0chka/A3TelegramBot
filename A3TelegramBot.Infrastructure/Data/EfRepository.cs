using Ardalis.SharedKernel;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace A3TelegramBot.Infrastructure.Data;

/// <inheritdoc cref="Ardalis.Specification.EntityFrameworkCore.RepositoryBase{T}" />
/// <summary>
///     Базовая реализация репозитория
/// </summary>
/// <param name="dbContext"> Контекст ef core </param>
/// <typeparam name="T"> Тип сущности </typeparam>
internal sealed class EfRepository<T>(DbContext dbContext):
    RepositoryBase<T>(dbContext), IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot;