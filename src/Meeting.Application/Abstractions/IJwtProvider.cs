using Meeting.Domain.Entities;

namespace Meeting.Application.Abstractions;

public interface IJwtProvider
{
    Task<string> GenerateAsync(Member member);
}