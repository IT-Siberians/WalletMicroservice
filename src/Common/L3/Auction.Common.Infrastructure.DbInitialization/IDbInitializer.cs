using System;
using System.Threading.Tasks;

namespace Auction.Common.Infrastructure.DbInitialization;

public interface IDbInitializer : IDisposable
{
    Task InitDatabaseAsync();
}
