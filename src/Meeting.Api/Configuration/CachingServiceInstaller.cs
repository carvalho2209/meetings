using Meeting.Domain.Repositories;
using Meeting.Persistence.Repository;
using Meeting.Persistence;
using Microsoft.Extensions.Caching.Distributed;

namespace Meeting.Api.Configuration;

public class CachingServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<MemberRepository>();
        services.AddScoped<IMemberRepository>(provider =>
        {
            var memberRepository = provider.GetService<MemberRepository>();
            var applicationDbContext = provider.GetService<ApplicationDbContext>();

            return new CachedMemberRepository(
                memberRepository!,
                provider.GetService<IDistributedCache>()!,
                applicationDbContext!);
        });

        services.AddStackExchangeRedisCache(redisOptions =>
        {
            string connection = configuration.GetConnectionString("Redis")!;

            redisOptions.Configuration = connection;
        });

        services.AddMemoryCache();
    }
}
