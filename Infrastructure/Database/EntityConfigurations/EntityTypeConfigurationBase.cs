using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Entities;

namespace Infrastructure.Database.EntityConfigurations
{
    public abstract class EntityTypeConfigurationBase<TEntity>(string schema = "dbo") : IEntityTypeConfiguration<TEntity> where TEntity : EntityBase
    {
        protected readonly string _schema = schema;
        protected readonly string _tableName = typeof(TEntity).Name.ToUpper();

        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.ToTable(_tableName, _schema);
            builder.Property("Key")
                .HasColumnName($"{_tableName}_ID")
                .IsRequired();
            builder.HasKey(e => e.Key);

            builder.Property(e => e.InsUserCode)
                .HasColumnName("INS_USER")
                .HasColumnType("uniqueidentifier");

            builder.Property(e => e.InsDate)
                .HasColumnName("INS_DATE")
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(e => e.UpdUserCode)
                .HasColumnName("UPD_USER")
                .HasColumnType("uniqueidentifier");

            builder.Property(e => e.UpdDate)
                .HasColumnName("UPD_DATE")
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(e => e.IsDelete)
                .HasColumnName("DEL_FLG")
                .HasColumnType("bit");
        }
    }
}
