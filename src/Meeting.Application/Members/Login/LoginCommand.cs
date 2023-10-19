using Meeting.Application.Abstractions.Messaging;

namespace Meeting.Application.Members.Login;

public record LoginCommand(string Email) : ICommand<string>;
