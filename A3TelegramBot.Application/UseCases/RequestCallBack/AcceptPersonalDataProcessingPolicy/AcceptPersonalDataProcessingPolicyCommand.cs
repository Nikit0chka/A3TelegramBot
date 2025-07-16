using Ardalis.SharedKernel;
using ErrorOr;

namespace A3TelegramBot.Application.UseCases.RequestCallBack.AcceptPersonalDataProcessingPolicy;

/// <inheritdoc cref="ICommand{TResponse}" />
/// <summary>
///     Команда принятия политики обработки персональных данных
/// </summary>
internal readonly record struct AcceptPersonalDataProcessingPolicyCommand(long ChatId):ICommand<ErrorOr<AcceptPersonalDataProcessingPolicyCommandResult>>;