# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["MarketNest.sln", "./"]
COPY ["src/MarketNest.Domain/MarketNest.Domain.csproj", "src/MarketNest.Domain/"]
COPY ["src/MarketNest.Application/MarketNest.Application.csproj", "src/MarketNest.Application/"]
COPY ["src/MarketNest.Infrastructure/MarketNest.Infrastructure.csproj", "src/MarketNest.Infrastructure/"]
COPY ["src/MarketNest.API/MarketNest.API.csproj", "src/MarketNest.API/"]

# Restore dependencies
RUN dotnet restore "src/MarketNest.API/MarketNest.API.csproj"

# Copy everything else
COPY . .

# Publish
WORKDIR "/src/src/MarketNest.API"
RUN dotnet publish "MarketNest.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS runtime
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MarketNest.API.dll"]
