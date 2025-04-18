# Base runtime image (.NET 9.0)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy solution and project files
COPY ["LeaveManagementSystem.sln", "./"]
COPY ["LeaveManagementSystem.WebApi/LeaveManagementSystem.WebApi.csproj", "LeaveManagementSystem.WebApi/"]
COPY ["LeaveManagementSystem.DATA/LeaveManagementSystem.DATA.csproj", "LeaveManagementSystem.DATA/"]
COPY ["LeaveManagementSystem.DOMAINE/LeaveManagementSystem.DOMAINE.csproj", "LeaveManagementSystem.DOMAINE/"]

# Restore packages
RUN dotnet restore "LeaveManagementSystem.sln"

# Install EF Core CLI tools globally
RUN dotnet tool install --global dotnet-ef --version 9.0.*
ENV PATH="$PATH:/root/.dotnet/tools"

# Copy all source code
COPY . .

# Build the project
WORKDIR "/src/LeaveManagementSystem.WebApi"
RUN dotnet build "LeaveManagementSystem.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Run EF Core migrations
WORKDIR "/src"
RUN mkdir -p /app/data
RUN dotnet ef migrations add InitialMigration \
    --project LeaveManagementSystem.DATA \
    --startup-project LeaveManagementSystem.WebApi \
    --configuration $BUILD_CONFIGURATION \
    --output-dir Migrations

RUN dotnet ef database update \
    --project LeaveManagementSystem.DATA \
    --startup-project LeaveManagementSystem.WebApi \
    --configuration $BUILD_CONFIGURATION

# Publish the project
FROM build AS publish
WORKDIR "/src/LeaveManagementSystem.WebApi"
RUN dotnet publish "LeaveManagementSystem.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
RUN mkdir -p /app/data
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LeaveManagementSystem.WebApi.dll"]