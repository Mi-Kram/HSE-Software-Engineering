# Базовый образ для выполнения (рантайм)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Образ для сборки (SDK)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Копируем все .csproj по отдельности и восстанавливаем зависимости
COPY PaymentsService.API/PaymentsService.API.csproj PaymentsService.API/
COPY PaymentsService.Application/PaymentsService.Application.csproj PaymentsService.Application/
COPY PaymentsService.Domain/PaymentsService.Domain.csproj PaymentsService.Domain/
COPY PaymentsService.Infrastructure/PaymentsService.Infrastructure.csproj PaymentsService.Infrastructure/

RUN dotnet restore PaymentsService.API/PaymentsService.API.csproj

# Копируем остальной исходный код
COPY PaymentsService.API/ PaymentsService.API/
COPY PaymentsService.Application/ PaymentsService.Application/
COPY PaymentsService.Domain/ PaymentsService.Domain/
COPY PaymentsService.Infrastructure/ PaymentsService.Infrastructure/

# Сборка проекта
WORKDIR /src/PaymentsService.API
RUN dotnet build PaymentsService.API.csproj -c $BUILD_CONFIGURATION -o /app/build

# Публикация проекта
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish PaymentsService.API.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Финальный образ
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PaymentsService.API.dll"]
