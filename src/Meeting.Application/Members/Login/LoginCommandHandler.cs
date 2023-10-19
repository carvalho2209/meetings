using Meeting.Application.Abstractions;
using Meeting.Application.Abstractions.Messaging;
using Meeting.Domain.Errors;
using Meeting.Domain.Repositories;
using Meeting.Domain.Shared;
using Meeting.Domain.ValueObjects;

namespace Meeting.Application.Members.Login;

public sealed class LoginCommandHandler : ICommandHandler<LoginCommand, string>
{
    private readonly IMemberRepository _memberRepository;
    public readonly IJwtProvider _JwtProvider;

    public LoginCommandHandler(IMemberRepository memberRepository, IJwtProvider jwtProvider)
    {
        _memberRepository = memberRepository;
        _JwtProvider = jwtProvider;
    }

    public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        Result<Email> email = Email.Create(request.Email);

        var member = await _memberRepository.GetByEmailAsync(email.Value, cancellationToken);

        if (member is null)
        {
            Result.Failure<string>(DomainErrors.Member.InvalidCredentials);
        }

        string token = _JwtProvider.Generate(member);

        return token;
    }
}
