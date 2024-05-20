# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Create the Images directory
RUN mkdir -p /app/Images

# Install SQL Server command-line tools
RUN apt-get update \
    && apt-get install -y curl gnupg \
    && curl -sL https://packages.microsoft.com/keys/microsoft.asc | apt-key add - \
    && curl -sL https://packages.microsoft.com/config/ubuntu/20.04/prod.list > /etc/apt/sources.list.d/mssql-release.list \
    && apt-get update \
    && ACCEPT_EULA=Y apt-get install -y msodbcsql17 mssql-tools \
    && echo 'export PATH="$PATH:/opt/mssql-tools/bin"' >> ~/.bashrc

# SDK image for building the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["NZWalks.API.csproj", "./"]
RUN dotnet restore "NZWalks.API.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "NZWalks.API.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "NZWalks.API.csproj" -c Release -o /app/publish

# Final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NZWalks.API.dll"]

# Health check to ensure the service is running
HEALTHCHECK --interval=30s --timeout=30s --start-period=5s --retries=3 \
    CMD curl -f http://localhost/ || exit 1
