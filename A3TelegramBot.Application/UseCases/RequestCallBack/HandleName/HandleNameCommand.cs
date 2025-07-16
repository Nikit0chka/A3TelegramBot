using Ardalis.SharedKernel;
using ErrorOr;

namespace A3TelegramBot.Application.UseCases.RequestCallBack.HandleName;

/// <inheritdoc cref="ICommand{TResponse}" />
/// <summary>
///     Команда ввода имени в
///     заявку на обратный звонок
/// </summary>
internal readonly record struct HandleNameCommand(long ChatId, string Name):ICommand<ErrorOr<Success>>;