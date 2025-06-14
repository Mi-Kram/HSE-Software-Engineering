# Базовый образ для выполнения (рантайм)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Образ для сборки (SDK)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Копируем все .csproj по отдельности и восстанавливаем зависимости
COPY OrdersService.API/OrdersService.API.csproj OrdersService.API/
COPY OrdersService.Application/OrdersService.Application.csproj OrdersService.Application/
COPY OrdersService.Domain/OrdersService.Domain.csproj OrdersService.Domain/
COPY OrdersService.Infrastructure/OrdersService.Infrastructure.csproj OrdersService.Infrastructure/

RUN dotnet restore OrdersService.API/OrdersService.API.csproj

# Копируем остальной исходный код
COPY OrdersService.API/ OrdersService.API/
COPY OrdersService.Application/ OrdersService.Application/
COPY OrdersService.Domain/ OrdersService.Domain/
COPY OrdersService.Infrastructure/ OrdersService.Infrastructure/

# Сборка проекта
WORKDIR /src/OrdersService.API
RUN dotnet build OrdersService.API.csproj -c $BUILD_CONFIGURATION -o /app/build

# Публикация проекта
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish OrdersService.API.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Финальный образ
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrdersService.API.dll"]
