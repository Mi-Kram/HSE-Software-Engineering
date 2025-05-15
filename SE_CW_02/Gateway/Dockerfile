# Базовый образ для выполнения (рантайм)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Образ для сборки (SDK)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Копируем все .csproj по отдельности и восстанавливаем зависимости
COPY Gateway.Api/Gateway.Api.csproj Gateway.Api/
COPY Gateway.Application/Gateway.Application.csproj Gateway.Application/
COPY Gateway.Domain/Gateway.Domain.csproj Gateway.Domain/
COPY Gateway.Infrastructure/Gateway.Infrastructure.csproj Gateway.Infrastructure/

RUN dotnet restore Gateway.Api/Gateway.Api.csproj

# Копируем остальной исходный код
COPY Gateway.Api/ Gateway.Api/
COPY Gateway.Application/ Gateway.Application/
COPY Gateway.Domain/ Gateway.Domain/
COPY Gateway.Infrastructure/ Gateway.Infrastructure/

# Сборка проекта
WORKDIR /src/Gateway.Api
RUN dotnet build Gateway.Api.csproj -c $BUILD_CONFIGURATION -o /app/build

# Публикация проекта
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish Gateway.Api.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Финальный образ
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gateway.Api.dll"]
