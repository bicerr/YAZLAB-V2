FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AuthService.Api/AuthService.Api.csproj", "AuthService.Api/"]
COPY ["AuthService.Application/AuthService.Application.csproj", "AuthService.Application/"]
COPY ["AuthService.Domain/AuthService.Domain.csproj", "AuthService.Domain/"]
COPY ["AuthService.Infrastructure/AuthService.Infrastructure.csproj", "AuthService.Infrastructure/"]
RUN dotnet restore "AuthService.Api/AuthService.Api.csproj"
COPY . .
RUN dotnet publish "AuthService.Api/AuthService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AuthService.Api.dll"]