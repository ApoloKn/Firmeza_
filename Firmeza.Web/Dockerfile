# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY Firmeza.sln .
COPY Firmeza.Web/Firmeza.Web.csproj Firmeza.Web/
COPY Firmeza.Data/Firmeza.Data.csproj Firmeza.Data/

# Restore dependencies
RUN dotnet restore Firmeza.Web/Firmeza.Web.csproj

# Copy everything else
COPY Firmeza.Web/ Firmeza.Web/
COPY Firmeza.Data/ Firmeza.Data/

# Build and publish
WORKDIR /src/Firmeza.Web
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 5001

ENV ASPNETCORE_URLS=http://+:5001

ENTRYPOINT ["dotnet", "Firmeza.Web.dll"]
