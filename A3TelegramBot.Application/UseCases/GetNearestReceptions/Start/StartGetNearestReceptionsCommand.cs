using Ardalis.SharedKernel;
using ErrorOr;

namespace A3TelegramBot.Application.UseCases.GetNearestReceptions.Start;

/// <inheritdoc cref="ICommand{TResponse}" />
/// <summary>
///     Команда для запуска процесса
///     поиска ближайших приемных пунктов
/// </summary>
internal readonly record struct StartGetNearestReceptionsCommand(long ChatId):ICommand<ErrorOr<Success>>;