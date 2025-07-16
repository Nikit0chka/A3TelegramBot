using Ardalis.Specification;

namespace A3TelegramBot.Domain.AggregateModels.UserSessionStatisticsAggregate.Specifications;

public class TheOnlyOneUserSessionStatisticsSpecification:SingleResultSpecification<UserSessionStatistics>
{
    public TheOnlyOneUserSessionStatisticsSpecification() { Query.Take(1); }
}