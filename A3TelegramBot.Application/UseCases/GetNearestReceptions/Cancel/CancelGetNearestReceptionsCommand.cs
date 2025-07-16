using Ardalis.SharedKernel;
using ErrorOr;

namespace A3TelegramBot.Application.UseCases.GetNearestReceptions.Cancel;

/// <inheritdoc cref="ICommand{TResponse}" />
/// <summary>
///     Команда для отмены процесса поиска
///     ближайших приемных пунктов
/// </summary>
internal readonly record struct CancelGetNearestReceptionsCommand(long ChatId):ICommand<ErrorOr<Success>>;