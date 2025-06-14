# Базовый образ для выполнения (рантайм)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Образ для сборки (SDK)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Копируем все .csproj по отдельности и восстанавливаем зависимости
COPY GatewayService.API/GatewayService.API.csproj GatewayService.API/
COPY GatewayService.Domain/GatewayService.Domain.csproj GatewayService.Domain/

RUN dotnet restore GatewayService.API/GatewayService.API.csproj

# Копируем остальной исходный код
COPY GatewayService.API/ GatewayService.API/
COPY GatewayService.Domain/ GatewayService.Domain/

# Сборка проекта
WORKDIR /src/GatewayService.API
RUN dotnet build GatewayService.API.csproj -c $BUILD_CONFIGURATION -o /app/build

# Публикация проекта
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish GatewayService.API.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Финальный образ
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GatewayService.API.dll"]
