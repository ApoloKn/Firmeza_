# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY Firmeza.sln .
COPY Firmeza.Api/Firmeza.Api.csproj Firmeza.Api/
COPY Firmeza.Data/Firmeza.Data.csproj Firmeza.Data/

# Restore dependencies
RUN dotnet restore Firmeza.Api/Firmeza.Api.csproj

# Copy everything else
COPY Firmeza.Api/ Firmeza.Api/
COPY Firmeza.Data/ Firmeza.Data/

# Build and publish
WORKDIR /src/Firmeza.Api
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Install curl for health check
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

EXPOSE 5000

ENV ASPNETCORE_URLS=http://+:5000

HEALTHCHECK --interval=30s --timeout=10s --retries=3 \
  CMD curl -f http://localhost:5000/health || exit 1

ENTRYPOINT ["dotnet", "Firmeza.Api.dll"]
