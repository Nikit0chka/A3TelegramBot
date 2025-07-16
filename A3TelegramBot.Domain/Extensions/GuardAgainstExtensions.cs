using Ardalis.GuardClauses;

namespace A3TelegramBot.Domain.Extensions;

/// <summary>
///     Методы расширения для guard against
///     <see cref="IGuardClause" />
/// </summary>
internal static class GuardAgainstExtensions
{
    /// <summary>
    ///     Проверка длины строки
    /// </summary>
    /// <param name="guardClause"> </param>
    /// <param name="input"> Входная строка </param>
    /// <param name="minLength"> Минимальная длина </param>
    /// <param name="maxLength"> Максимальная длина </param>
    /// <param name="parameterName"> Название параметра </param>
    /// <param name="message"> Сообщения для отображения при ошибке </param>
    /// <exception cref="ArgumentException"> Выбрасывается, если валидация не прошла </exception>
    internal static void StringLengthOutOfRange(this IGuardClause guardClause, string input,
                                                int minLength,
                                                int maxLength,
                                                string parameterName,
                                                string? message)
    {
        guardClause.Negative(maxLength - minLength,
                             "min or max length",
                             "Min length must be equal or less than max length.");

        try
        {
            guardClause.StringTooShort(input, minLength, parameterName, message);
            guardClause.StringTooLong(input, maxLength, parameterName, message);
        }
        catch (ArgumentException)
        {
            throw new ArgumentException(message);
        }
    }
}