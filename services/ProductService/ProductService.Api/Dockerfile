FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ProductService.Api/ProductService.Api.csproj", "ProductService.Api/"]
COPY ["ProductService.Application/ProductService.Application.csproj", "ProductService.Application/"]
COPY ["ProductService.Domain/ProductService.Domain.csproj", "ProductService.Domain/"]
COPY ["ProductService.Infrastructure/ProductService.Infrastructure.csproj", "ProductService.Infrastructure/"]
RUN dotnet restore "ProductService.Api/ProductService.Api.csproj"
COPY . .
RUN dotnet publish "ProductService.Api/ProductService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ProductService.Api.dll"]