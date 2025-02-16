using Microsoft.EntityFrameworkCore;
using TimeWaster.Data;

namespace TimeWaster.Web.HostedServices;

public class MigrationHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public MigrationHostedService(IServiceProvider provider)
    {
        _serviceProvider = provider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var context = _serviceProvider.CreateScope()
            .ServiceProvider.GetService<TimeWasterDbContext>();
        
        if (context is null)
        {
            throw new Exception();
        }

        return context.Database.MigrateAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}