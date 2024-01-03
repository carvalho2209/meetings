namespace Meeting.Application.Abstractions;

public interface ISystemTimeProvider
{
    public DateTime UtcNow { get; }
}