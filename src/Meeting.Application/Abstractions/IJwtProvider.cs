using Meeting.Domain.Entities;

namespace Meeting.Application.Abstractions;

public interface IJwtProvider
{
    string Generate(Member member);
}