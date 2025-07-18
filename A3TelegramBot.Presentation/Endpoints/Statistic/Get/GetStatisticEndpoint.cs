using A3TelegramBot.Application.UseCases.UserSessionStatistic.GetSummaryStatistic;
using A3TelegramBot.Presentation.Endpoints.Base;
using FastEndpoints;
using MediatR;

namespace A3TelegramBot.Presentation.Endpoints.Statistic.Get;

/// <inheritdoc />
/// <summary>
///     Эндпоинт для получения статистики
/// </summary>
/// <param name="mediator"> </param>
public class GetAdvertisementEndpoint(IMediator mediator):Endpoint<GetStatisticRequest, SummaryStatisticsDto>
{
    public override void Configure()
    {
        Get(BaseEndpointsRoutes.StatisticRoute);

        Description(static b => b
                        .WithTags("Statistics")
                        .Produces<SummaryStatisticsDto>(200, "application/json")
                        .ProducesProblem(400)
                        .ProducesProblem(401));

        // Явно указываем параметры запроса для Swagger
        Summary(static summary =>
        {
            summary.Description = "Получить статистику работы с ботом за период";

            summary.Params[nameof(GetStatisticRequest.DateStart)] =
                "Начальная дата периода в формате YYYY-MM-DD";

            summary.Params[nameof(GetStatisticRequest.DateEnd)] =
                "Конечная дата периода в формате YYYY-MM-DD";
        });
    }

    public override async Task HandleAsync(GetStatisticRequest request, CancellationToken cancellationToken)
    {
        var summaryStatistics = await mediator.Send(new GetSummaryStatisticQuery(request.DateStart, request.DateEnd), cancellationToken);

        //TODO: обработать ошибку

        await SendOkAsync(summaryStatistics, cancellationToken);
    }
}