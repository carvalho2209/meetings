﻿using Meeting.Application.Abstractions;
using Meeting.Domain.Entities;

namespace Meeting.Infrastructure.Services;

public sealed class EmailService : IEmailService
{
    public Task SendWelcomeEmailAsync(Member member, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public Task SendInvitationSentEmailAsync(Member member, Domain.Entities.Meeting meeting,
        CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public Task SendInvitationAcceptedEmailAsync(Domain.Entities.Meeting meeting,
        CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public Task SendGatheringCancelledEmailAsync(Attendee attendee, CancellationToken cancellationToken = default) =>
        Task.CompletedTask;
}
