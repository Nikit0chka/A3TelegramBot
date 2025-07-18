using A3TelegramBot.Application.Contracts;
using ErrorOr;
using MediatR;

namespace A3TelegramBot.Application.Extensions;

/// <summary>
///     Методы расширения при работе
///     с заявкой на обратный звонок
/// </summary>
internal static class MediatorExtensions
{
    public static async Task<TResult> SendMediatorRequest<TResult>(
        this IMediator mediator,
        IRequest<TResult> request,
        long chatId,
        ITelegramResponseService responseService,
        CancellationToken cancellationToken)
        where TResult : IErrorOr
    {
        var result = await mediator.Send(request, cancellationToken);

        if (!result.IsError || result.Errors == null)
            return result;

        var errorMessage = string.Join("\n", result.Errors.Select(static error => error.Description));
        await responseService.SendErrorAsync(chatId, errorMessage, cancellationToken);

        return result;
    }
}