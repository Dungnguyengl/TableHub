using Core.Enum;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Database.EntityConfigurations
{
    public class TableEntityTypeConfiguration : EntityTypeConfigurationBase<Table>
    {
        public override void Configure(EntityTypeBuilder<Table> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.Status)
                .HasColumnType("nvarchar(10)")
                .HasConversion(new EnumToStringConverter<TableStatus>());
        }
    }
}
