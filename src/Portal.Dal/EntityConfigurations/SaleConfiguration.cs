using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.Entities;

namespace Portal.Dal.EntityConfigurations;

public class SaleConfiguration : IEntityTypeConfiguration<Sale> {
    public void Configure(EntityTypeBuilder<Sale> builder) {
        builder.ToTable(nameof(Sale));
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.DateTimeSale)
            .IsRequired();

        builder
            .Property(x => x.Amount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.HasIndex(x => x.DateTimeSale);
    }
}
