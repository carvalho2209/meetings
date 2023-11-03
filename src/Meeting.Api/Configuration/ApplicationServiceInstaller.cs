using MediatR;
using Meeting.Application.Behaviors;
using Meeting.Infrastructure.Idempotence;

namespace Meeting.Api.Configuration;

public class ApplicationServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

        services.AddScoped(typeof(INotificationHandler<>), typeof(IdempotentDomainEventHandler<>));
    }
}
