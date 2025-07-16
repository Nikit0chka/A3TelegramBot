using A3TelegramBot.Application.Dto;
using Ardalis.SharedKernel;
using ErrorOr;

namespace A3TelegramBot.Application.UseCases.GetNearestReceptions.GetNearestReceptions;

/// <inheritdoc cref="ICommand{TResponse}" />
/// <summary>
///     Команда для получения списка
///     ближайших приемных пунктов
/// </summary>
internal readonly record struct GetNearestReceptionsCommand(long ChatId, double Latitude, double Longitude):ICommand<ErrorOr<IReadOnlyCollection<ReceptionInfo>>>;