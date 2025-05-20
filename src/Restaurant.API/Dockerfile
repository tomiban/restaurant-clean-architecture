FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Restaurant.API/Restaurant.API.csproj", "Restaurant.API/"]
COPY ["src/Restaurant.Domain/Restaurant.Domain.csproj", "Restaurant.Domain/"]
COPY ["src/Restaurant.Application/Restaurant.Application.csproj", "Restaurant.Application/"]
COPY ["src/Restaurant.Infrastructure/Restaurant.Infrastructure.csproj", "Restaurant.Infrastructure/"]
RUN dotnet restore "Restaurant.API/Restaurant.API.csproj"
COPY src/ ./
WORKDIR "/src/Restaurant.API"
RUN dotnet build "./Restaurant.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Restaurant.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Restaurant.API.dll"]
