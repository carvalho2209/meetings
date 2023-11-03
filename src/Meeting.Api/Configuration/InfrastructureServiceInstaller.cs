using Meeting.Application.Abstractions;
using Meeting.Domain.Repositories;
using Meeting.Infrastructure.Authentication;
using Meeting.Infrastructure.Services;
using Meeting.Persistence;
using Meeting.Persistence.Interceptors;
using Meeting.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Meeting.Api.Configuration;

public class InfrastructureServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IMeetingRepository, MeetingRepository>();
        services.AddScoped<IAttendeeRepository, AttendeeRepository>();
        services.AddScoped<IInvitationRepository, InvitationRepository>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IPermissionService, PermissionService>();

        services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();

        services.AddSingleton<UpdateAuditableEntitiesInterceptor>();

        services.AddDbContext<ApplicationDbContext>(optionsBuilder =>
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });
    }
}
