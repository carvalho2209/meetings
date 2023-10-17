using Meeting.Domain.Shared;

namespace Meeting.Domain.Errors;

public static class DomainErrors
{
    public static class Member
    {
        public static readonly Error EmailAlreadyInUse = new(
            "Member.EmailAlreadyInUse",
            "The specified email is already in use.");
    }

    public static class Invitation
    {
        public static readonly Func<Guid, Error> AlreadyAccepted = id => new Error(
            "Invitation.AlreadyAccepted",
            $"The invitation with Id {id} has already been accepted");
    }

    public static class Meeting
    {
        public static readonly Func<Guid, Error> NotFound = id => new Error(
            "Meeting.NotFound",
            $"The gathering with Id {id} was not found");

        public static readonly Error InvitingCreator = new(
            "Meeting.InvitingCreator",
            "Can't send invitation to the gathering creator.");

        public static readonly Error AlreadyPassed = new(
            "Meeting.AlreadyPassed",
            "Can't send invitation for gathering in the past.");

        public static readonly Error Expired = new(
            "Meeting.Expired",
            "Can't accept invitation for expired gathering.");
    }

    public static class Email
    {
        public static readonly Error Empty = new(
            "Email.Empty",
            "Email is empty");

        public static readonly Error InvalidFormat = new(
            "Email.InvalidFormat",
            "Email format is invalid");
    }

    public static class FirstName
    {
        public static readonly Error Empty = new(
            "FirstName.Empty",
            "First name is empty");

        public static readonly Error TooLong = new(
            "LastName.TooLong",
            "FirstName name is too long");
    }

    public static class LastName
    {
        public static readonly Error Empty = new(
            "LastName.Empty",
            "Last name is empty");

        public static readonly Error TooLong = new(
            "LastName.TooLong",
            "Last name is too long");
    }
}