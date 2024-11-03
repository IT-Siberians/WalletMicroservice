using System;
using System.Threading.Tasks;

namespace Auction.Common.Presentation.Initialization;

public interface IDbInitializer : IDisposable
{
    Task InitDatabaseAsync();
}
