using Ardalis.SharedKernel;
using ErrorOr;

namespace A3TelegramBot.Application.UseCases.RequestCallBack.Cancel;

/// <inheritdoc cref="ICommand{TResponse}" />
/// <summary>
///     Команда отмены заявки на обратный звонок
/// </summary>
internal readonly record struct CancelCallbackStateCommand(long ChatId):ICommand<ErrorOr<Success>>;