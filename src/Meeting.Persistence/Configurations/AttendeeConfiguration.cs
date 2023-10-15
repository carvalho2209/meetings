using Meeting.Domain.Entities;
using Meeting.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Meeting.Persistence.Configurations;

internal sealed class AttendeeConfiguration : IEntityTypeConfiguration<Attendee>
{
    public void Configure(EntityTypeBuilder<Attendee> builder)
    {
        builder.ToTable(TableNames.Attendees);
        
        builder.HasKey(x => new {x.MeetingId, x.MemberId });

        builder
            .HasOne<Member>()
            .WithMany()
            .HasForeignKey(x => x.MeetingId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
