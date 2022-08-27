#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Identity/Identity.Api/Identity.Api.csproj", "Services/Identity/Identity.Api/"]
COPY ["Services/Identity/Identity.Application/Identity.Application.csproj", "Services/Identity/Identity.Application/"]
COPY ["Platform/Persistence.EfCore/Persistence.EfCore.csproj", "Platform/Persistence.EfCore/"]
COPY ["Platform/SharedCommon/SharedCommon.csproj", "Platform/SharedCommon/"]
COPY ["Platform/SharedEventBus/SharedEventBus.csproj", "Platform/SharedEventBus/"]
COPY ["Platform/EventMessageBus/EventBusMessage.csproj", "Platform/EventMessageBus/"]
COPY ["Services/Identity/Identity.Infrastructure/Identity.Infrastructure.csproj", "Services/Identity/Identity.Infrastructure/"]
COPY ["Services/Identity/Identity.Domain/Identity.Domain.csproj", "Services/Identity/Identity.Domain/"]
RUN dotnet restore "Services/Identity/Identity.Api/Identity.Api.csproj"
COPY . .
WORKDIR "/src/Services/Identity/Identity.Api"
RUN dotnet build "Identity.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Identity.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.Api.dll"]