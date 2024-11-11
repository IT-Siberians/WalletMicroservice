using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Auction.Common.Infrastructure.DbInitialization;

public static class InitializationHelper
{
    public static async Task InitAsync<T>(this IHost host)
        where T : IDbInitializer
    {
        using var scope = host.Services.CreateScope();
        using var dbInitializer = scope.ServiceProvider.GetRequiredService<T>();

        await dbInitializer.InitDatabaseAsync();
    }
}
