using Ardalis.SharedKernel;
using ErrorOr;

namespace A3TelegramBot.Application.UseCases.RequestCallBack.HandlePhone;

/// <inheritdoc cref="ICommand{TResponse}" />
/// <summary>
///     Команда ввода номера телефона в
///     заявку на обратный звонок
/// </summary>
internal readonly record struct HandlePhoneCommand(long ChatId, string PhoneNumber):ICommand<ErrorOr<Success>>;