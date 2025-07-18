using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.CallBackRequestEntity;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.CallBackRequestEntity.ValueObjects.PhoneNumberValueObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace A3TelegramBot.Infrastructure.Data.Configs;

/// <inheritdoc />
/// <summary>
///     Конфигурация заявки
///     на обратный звонок
/// </summary>
public class CallBackRequestConfiguration:IEntityTypeConfiguration<CallBackRequest>
{
    public void Configure(EntityTypeBuilder<CallBackRequest> builder)
    {
        builder.Property(static callbackRequest => callbackRequest.Phone)
            .HasConversion(static phone => phone.HasValue? phone.Value.Value : null,
                           static phoneString => string.IsNullOrEmpty(phoneString)
                               ? null
                               : PhoneNumber.Create(phoneString).IsError
                                   ? null
                                   : PhoneNumber.Create(phoneString).Value)
            .HasMaxLength(PhoneNumberConstants.PhoneNumberMaxLength).IsRequired(false);

        builder.Property(static callbackRequest => callbackRequest.Name).HasMaxLength(CallBackRequestConstants.NameMaxLength);
    }
}