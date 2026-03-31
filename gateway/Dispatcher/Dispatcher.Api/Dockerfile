FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Dispatcher.Api/Dispatcher.Api.csproj", "Dispatcher.Api/"]
COPY ["Dispatcher.Application/Dispatcher.Application.csproj", "Dispatcher.Application/"]
COPY ["Dispatcher.Domain/Dispatcher.Domain.csproj", "Dispatcher.Domain/"]
COPY ["Dispatcher.Infrastructure/Dispatcher.Infrastructure.csproj", "Dispatcher.Infrastructure/"]
RUN dotnet restore "Dispatcher.Api/Dispatcher.Api.csproj"
COPY . .
RUN dotnet publish "Dispatcher.Api/Dispatcher.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Dispatcher.Api.dll"]