using Ardalis.SharedKernel;
using ErrorOr;

namespace A3TelegramBot.Application.UseCases.RequestCallBack.Start;

/// <inheritdoc cref="ICommand{TResponse}" />
/// <summary>
///     Команда старта заявки на обратный звонок
/// </summary>
internal readonly record struct StartCallBackRequestCommand(long ChatId):ICommand<ErrorOr<Success>>;