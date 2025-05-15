# # См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# # Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
# FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# USER $APP_UID
# WORKDIR /app
# EXPOSE 8080
# EXPOSE 8081

# ENV FAnalysis.COMPARISON_FILE "/app/lib/jplag.jar"

# USER root

# # Установка OpenJDK 21 JRE с очисткой кэша
# RUN apt-get update && \
#     apt-get install -y openjdk-21-jre && \
#     apt-get clean && \
#     rm -rf /var/lib/apt/lists/*
    

# # Этот этап используется для сборки проекта службы
# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# ARG BUILD_CONFIGURATION=Release
# WORKDIR /src
# COPY ["FileAnalysisService.Api.csproj", "FileAnalysisService.Api/"]
# RUN dotnet restore "./FileAnalysisService.Api/FileAnalysisService.Api.csproj"
# COPY ./../FileAnalysisService.Api/ FileAnalysisService.Api/
# COPY ./../FileAnalysisService.Application/ FileAnalysisService.Application/
# COPY ./../FileAnalysisService.Domain/ FileAnalysisService.Domain/
# COPY ./../FileAnalysisService.Infrastructure/ FileAnalysisService.Infrastructure/
# WORKDIR "/src/FileAnalysisService.Api"
# RUN dotnet build "./FileAnalysisService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# # Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
# FROM build AS publish
# ARG BUILD_CONFIGURATION=Release
# RUN dotnet publish "./FileAnalysisService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# # Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
# FROM base AS final

# WORKDIR /app
# COPY --from=publish /app/publish .

# # Копируем jplag.jar в папку /app/lib
# RUN mkdir -p /app/lib
# COPY ./../jplag.jar /app/lib/

# ENTRYPOINT ["dotnet", "FileAnalysisService.Api.dll"]