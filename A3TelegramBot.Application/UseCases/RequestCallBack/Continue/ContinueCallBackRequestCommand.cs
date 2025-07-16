using Ardalis.SharedKernel;
using ErrorOr;

namespace A3TelegramBot.Application.UseCases.RequestCallBack.Continue;

/// <inheritdoc cref="ICommand{TResponse}" />
/// <summary>
///     Команда возобновления
///     заявки на обратный звонок
/// </summary>
internal readonly record struct ContinueCallBackRequestCommand(long ChatId):ICommand<ErrorOr<Success>>;