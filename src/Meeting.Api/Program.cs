using Meeting.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .InstallServices(
        builder.Configuration, 
        typeof(IServiceInstaller).Assembly);

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