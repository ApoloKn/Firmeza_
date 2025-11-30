# Build and test stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY Firmeza.sln .
COPY Firmeza.Tests/Firmeza.Tests.csproj Firmeza.Tests/
COPY Firmeza.Api/Firmeza.Api.csproj Firmeza.Api/
COPY Firmeza.Data/Firmeza.Data.csproj Firmeza.Data/
COPY Firmeza.Web/Firmeza.Web.csproj Firmeza.Web/

# Restore dependencies
RUN dotnet restore Firmeza.Tests/Firmeza.Tests.csproj

# Copy everything else
COPY . .

# Build the test project
WORKDIR /src/Firmeza.Tests
RUN dotnet build -c Release --no-restore

# Run tests
ENTRYPOINT ["dotnet", "test", "--no-build", "-c", "Release", "--logger", "console;verbosity=detailed"]
