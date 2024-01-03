using Meeting.Application.Abstractions;

namespace Meeting.Infrastructure.Services;

public sealed class SystemTimeProvider : ISystemTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}

