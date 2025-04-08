# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MotoLocadora.Api/MotoLocadora.Api.csproj", "MotoLocadora.Api/"]
COPY ["MotoLocadora.Application/MotoLocadora.Application.csproj", "MotoLocadora.Application/"]
COPY ["MotoLocadora.BuildingBlocks/MotoLocadora.BuildingBlocks.csproj", "MotoLocadora.BuildingBlocks/"]
COPY ["MotoLocadora.Domain/MotoLocadora.Domain.csproj", "MotoLocadora.Domain/"]
COPY ["MotoLocadora.Infraestructure.Ioc/MotoLocadora.Infraestructure.Ioc.csproj", "MotoLocadora.Infraestructure.Ioc/"]
COPY ["MotoLocadora.Infrastructure/MotoLocadora.Infrastructure.csproj", "MotoLocadora.Infrastructure/"]
RUN dotnet restore "./MotoLocadora.Api/MotoLocadora.Api.csproj"
COPY . .
WORKDIR "/src/MotoLocadora.Api"
RUN dotnet build "./MotoLocadora.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MotoLocadora.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MotoLocadora.Api.dll"]