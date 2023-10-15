using Meeting.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Meeting.Persistence.Configurations;

internal sealed class MeetingConfiguration : IEntityTypeConfiguration<Meeting.Domain.Entities.Meeting> 
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Meeting> builder) 
    {
        builder.ToTable(TableNames.Meetings);

        builder.HasKey(x => x.Id);

        builder
            .HasOne(x => x.Creator)
            .WithMany();

        builder
            .HasMany(x => x.Invitations)
            .WithOne()
            .HasForeignKey(x => x.MeetingId);

        builder
            .HasMany(x => x.Attendees)
            .WithOne()
            .HasForeignKey(x => x.MeetingId);
    }
}
