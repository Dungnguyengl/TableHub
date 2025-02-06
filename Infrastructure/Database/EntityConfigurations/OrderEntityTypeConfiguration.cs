using Core.Enum;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Database.EntityConfigurations
{
    public class OrderEntityTypeConfiguration : EntityTypeConfigurationBase<Order>
    {
        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Status)
                .HasColumnType("nvarchar(10)")
                .HasConversion(new EnumToStringConverter<OrderStatus>());
        }
    }
}
