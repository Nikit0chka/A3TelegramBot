using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.CallBackRequestEntity;
using Ardalis.SharedKernel;
using ErrorOr;

namespace A3TelegramBot.Application.UseCases.RequestCallBack.GetState;

/// <inheritdoc cref="ICommand{TResponse}" />
/// <summary>
///     Команда получения состояния
///     заявки на обратный звонок
/// </summary>
internal readonly record struct GetCallBackStateCommand(long ChatId):ICommand<ErrorOr<CallBackRequestStatus?>>;