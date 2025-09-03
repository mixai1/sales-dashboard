FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Portal.Application/Portal.Application.csproj", "Portal.Application/"]
COPY ["Portal.Dal/Portal.Dal.csproj", "Portal.Dal/"]
COPY ["Portal.Entities/Portal.Entities.csproj", "Portal.Entities/"]
RUN dotnet restore "./Portal.Application/Portal.Application.csproj"
COPY . .
WORKDIR "/src/Portal.Application"
RUN dotnet build "./Portal.Application.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Portal.Application.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Portal.Application.dll"]