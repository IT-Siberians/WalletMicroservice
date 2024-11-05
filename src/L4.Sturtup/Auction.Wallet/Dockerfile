# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["L4.Sturtup/Auction.Wallet/Auction.Wallet.csproj", "L4.Sturtup/Auction.Wallet/"]
COPY ["Common/L3/Auction.Common.Infrastructure.Initialization/Auction.Common.Infrastructure.Initialization.csproj", "Common/L3/Auction.Common.Infrastructure.Initialization/"]
COPY ["Common/L3/Auction.Common.Infrastructure.Repositories/Auction.Common.Infrastructure.Repositories.csproj", "Common/L3/Auction.Common.Infrastructure.Repositories/"]
COPY ["Common/L2/Auction.Common.Application.L2.Interfaces/Auction.Common.Application.L2.Interfaces.csproj", "Common/L2/Auction.Common.Application.L2.Interfaces/"]
COPY ["Common/L2/Auction.Common.Application.L1.Models/Auction.Common.Application.L1.Models.csproj", "Common/L2/Auction.Common.Application.L1.Models/"]
COPY ["Common/L1/Auction.Common.Domain/Auction.Common.Domain.csproj", "Common/L1/Auction.Common.Domain/"]
COPY ["Common/L3/Auction.Common.Presentation.Validation/Auction.Common.Presentation.Validation.csproj", "Common/L3/Auction.Common.Presentation.Validation/"]
COPY ["L3.Infrastructure/Auction.Wallet.Infrastructure.Repositories/Auction.Wallet.Infrastructure.Repositories.csproj", "L3.Infrastructure/Auction.Wallet.Infrastructure.Repositories/"]
COPY ["L2.Application/Auction.Wallet.Application.L2.Interfaces/Auction.Wallet.Application.L2.Interfaces.csproj", "L2.Application/Auction.Wallet.Application.L2.Interfaces/"]
COPY ["L2.Application/Auction.Wallet.Application.L1.Models/Auction.Wallet.Application.L1.Models.csproj", "L2.Application/Auction.Wallet.Application.L1.Models/"]
COPY ["L1.Domain/Auction.Wallet.Domain/Auction.Wallet.Domain.csproj", "L1.Domain/Auction.Wallet.Domain/"]
COPY ["L3.Infrastructure/Auction.Wallet.Infrastructure.EntityFramework/Auction.Wallet.Infrastructure.EntityFramework.csproj", "L3.Infrastructure/Auction.Wallet.Infrastructure.EntityFramework/"]
COPY ["L3.Presentation/Auction.Wallet.Presentation.Validation/Auction.Wallet.Presentation.Validation.csproj", "L3.Presentation/Auction.Wallet.Presentation.Validation/"]
COPY ["L3.Presentation/Auction.Wallet.Presentation.WebApi/Auction.Wallet.Presentation.WebApi.csproj", "L3.Presentation/Auction.Wallet.Presentation.WebApi/"]
COPY ["Common/L3/Auction.Common.Presentation.WebApi/Auction.Common.Presentation.WebApi.csproj", "Common/L3/Auction.Common.Presentation.WebApi/"]
COPY ["Common/L2/Auction.Common.Application.L3.Logic/Auction.Common.Application.L3.Logic.csproj", "Common/L2/Auction.Common.Application.L3.Logic/"]
COPY ["L2.Application/Auction.Wallet.Application.L3.Logic/Auction.Wallet.Application.L3.Logic.csproj", "L2.Application/Auction.Wallet.Application.L3.Logic/"]
RUN dotnet restore "./L4.Sturtup/Auction.Wallet/Auction.Wallet.csproj"
COPY . .
WORKDIR "/src/L4.Sturtup/Auction.Wallet"
RUN dotnet build "./Auction.Wallet.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Auction.Wallet.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Auction.Wallet.dll"]