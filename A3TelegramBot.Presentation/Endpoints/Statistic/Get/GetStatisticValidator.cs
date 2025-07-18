using FastEndpoints;
using FluentValidation;

namespace A3TelegramBot.Presentation.Endpoints.Statistic.Get;

internal class GetStatisticValidator:Validator<GetStatisticRequest>
{
    public GetStatisticValidator()
    {
        RuleFor(static request => request.DateEnd)
            .GreaterThan(static request => request.DateStart)
            .WithMessage("Дата начала должна быть раньше даты конца");
    }
}