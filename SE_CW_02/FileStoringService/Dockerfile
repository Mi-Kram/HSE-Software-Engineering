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
COPY FileStoringService.Api/FileStoringService.Api.csproj FileStoringService.Api/
COPY FileStoringService.Application/FileStoringService.Application.csproj FileStoringService.Application/
COPY FileStoringService.Domain/FileStoringService.Domain.csproj FileStoringService.Domain/
COPY FileStoringService.Infrastructure/FileStoringService.Infrastructure.csproj FileStoringService.Infrastructure/

RUN dotnet restore FileStoringService.Api/FileStoringService.Api.csproj

# Копируем остальной исходный код
COPY FileStoringService.Api/ FileStoringService.Api/
COPY FileStoringService.Application/ FileStoringService.Application/
COPY FileStoringService.Domain/ FileStoringService.Domain/
COPY FileStoringService.Infrastructure/ FileStoringService.Infrastructure/

# Сборка проекта
WORKDIR /src/FileStoringService.Api
RUN dotnet build FileStoringService.Api.csproj -c $BUILD_CONFIGURATION -o /app/build

# Публикация проекта
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish FileStoringService.Api.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Финальный образ
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FileStoringService.Api.dll"]
