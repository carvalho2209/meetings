using Meeting.Persistence.Constants;
using Meeting.Persistence.OutBox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Meeting.Persistence.Configurations;

internal sealed class OutBoxMessageConfiguration : IEntityTypeConfiguration<OutBoxMessage>
{
    public void Configure(EntityTypeBuilder<OutBoxMessage> builder)
    {
        builder.ToTable(TableNames.OutBoxMessages);

        builder.HasKey(t => t.Id);
    }
}
