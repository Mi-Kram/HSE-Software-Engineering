# Базовый образ для выполнения (рантайм)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Образ для сборки (SDK)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Копируем все .csproj по отдельности и восстанавливаем зависимости
COPY OrderStatusChangeNotifier.API/OrderStatusChangeNotifier.API.csproj OrderStatusChangeNotifier.API/
COPY OrderStatusChangeNotifier.Application/OrderStatusChangeNotifier.Application.csproj OrderStatusChangeNotifier.Application/
COPY OrderStatusChangeNotifier.Domain/OrderStatusChangeNotifier.Domain.csproj OrderStatusChangeNotifier.Domain/
COPY OrderStatusChangeNotifier.Infrastructure/OrderStatusChangeNotifier.Infrastructure.csproj OrderStatusChangeNotifier.Infrastructure/

RUN dotnet restore OrderStatusChangeNotifier.API/OrderStatusChangeNotifier.API.csproj

# Копируем остальной исходный код
COPY OrderStatusChangeNotifier.API/ OrderStatusChangeNotifier.API/
COPY OrderStatusChangeNotifier.Application/ OrderStatusChangeNotifier.Application/
COPY OrderStatusChangeNotifier.Domain/ OrderStatusChangeNotifier.Domain/
COPY OrderStatusChangeNotifier.Infrastructure/ OrderStatusChangeNotifier.Infrastructure/

# Сборка проекта
WORKDIR /src/OrderStatusChangeNotifier.API
RUN dotnet build OrderStatusChangeNotifier.API.csproj -c $BUILD_CONFIGURATION -o /app/build

# Публикация проекта
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish OrderStatusChangeNotifier.API.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Финальный образ
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderStatusChangeNotifier.API.dll"]
