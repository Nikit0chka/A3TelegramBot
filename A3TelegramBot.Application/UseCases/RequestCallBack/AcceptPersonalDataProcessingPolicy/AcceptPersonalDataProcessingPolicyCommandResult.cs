namespace A3TelegramBot.Application.UseCases.RequestCallBack.AcceptPersonalDataProcessingPolicy;

/// <inheritdoc />
/// <summary>
///     Результат выполнения команды принятия политики
/// </summary>
/// <param name="CallBackRequestUserName"> Имя пользователя отправившего заявку на обратный звонок </param>
/// <param name="CallBackRequestUserPhone"> Номер пользователя отправившего заявку на обратный звонок </param>
internal readonly record struct AcceptPersonalDataProcessingPolicyCommandResult(string CallBackRequestUserName, string CallBackRequestUserPhone);