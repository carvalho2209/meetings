using MediatR;
using Meeting.Application.Abstractions;
using Meeting.Application.Behaviors;
using Meeting.Domain.Repositories;
using Meeting.Infrastructure.BackgroundJobs;
using Meeting.Infrastructure.Idempotence;
using Meeting.Infrastructure.Services;
using Meeting.Persistence;
using Meeting.Persistence.Interceptors;
using Meeting.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<MemberRepository>();
builder.Services.AddScoped<IMemberRepository>(provider =>
{
    var memberRepository = provider.GetService<MemberRepository>();
    var applicationDbContext = provider.GetService<ApplicationDbContext>();

    return new CachedMemberRepository(
        memberRepository!,
        provider.GetService<IDistributedCache>()!,
        applicationDbContext!);
});

builder.Services.AddStackExchangeRedisCache(redisOptions =>
{
    string connection = builder.Configuration.GetConnectionString("Redis")!;

    redisOptions.Configuration = connection;
});

//builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IMeetingRepository, MeetingRepository>();
builder.Services.AddScoped<IAttendeeRepository, AttendeeRepository>();
builder.Services.AddScoped<IInvitationRepository, InvitationRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddMemoryCache();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));


builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

builder.Services.AddScoped(typeof(INotificationHandler<>), typeof(IdempotentDomainEventHandler<>));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

builder.Services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();
builder.Services.AddSingleton<UpdateAuditableEntitiesInterceptor>();

builder.Services.AddDbContext<ApplicationDbContext>(
    (sp, optionsBuilder) =>
    {
        optionsBuilder.UseSqlServer(connectionString);
    });

builder.Services.AddQuartz(configure =>
{
    var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

    configure
        .AddJob<ProcessOutboxMessagesJob>(jobKey)
        .AddTrigger(
            trigger =>
                trigger.ForJob(jobKey)
                    .WithSimpleSchedule(schedule =>
                        schedule.WithIntervalInSeconds(10)
                            .RepeatForever()));
});

builder.Services.AddQuartzHostedService();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();