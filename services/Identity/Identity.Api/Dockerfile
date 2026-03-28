FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8011

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["services/Identity/Identity.Api/Identity.Api.csproj", "services/Identity/Identity.Api/"]
COPY ["Infrastructure/Common.Logging/Common.Logging.csproj", "Infrastructure/Common.Logging/"]
RUN dotnet restore "./services/Identity/Identity.Api/Identity.Api.csproj"
COPY . .
WORKDIR "/src/services/Identity/Identity.Api"
RUN dotnet build "./Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.Api.dll"]
