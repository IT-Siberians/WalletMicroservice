using Auction.Common.Infrastructure.DbInitialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Auction.Common.Infrastructure.DbInitialization;

public static class MigrationHelper
{
    public static async Task MigrateAsync<T>(this IHost host)
        where T : DbContext
    {
        using var scope = host.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<T>();

        await dbContext.Database.MigrateAsync();
    }
}
