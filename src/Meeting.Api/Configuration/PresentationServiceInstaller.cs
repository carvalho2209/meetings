namespace Meeting.Api.Configuration;

public class PresentationServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();

        services.AddControllers();

        services.AddSwaggerGen();
    }
}
