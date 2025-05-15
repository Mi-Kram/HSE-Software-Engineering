# -------- Этап сборки --------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Копируем проекты и восстанавливаем зависимости
COPY FileAnalysisService.Api/FileAnalysisService.Api.csproj FileAnalysisService.Api/
COPY FileAnalysisService.Application/FileAnalysisService.Application.csproj FileAnalysisService.Application/
COPY FileAnalysisService.Domain/FileAnalysisService.Domain.csproj FileAnalysisService.Domain/
COPY FileAnalysisService.Infrastructure/FileAnalysisService.Infrastructure.csproj FileAnalysisService.Infrastructure/

RUN dotnet restore FileAnalysisService.Api/FileAnalysisService.Api.csproj

# Копируем остальной исходный код
COPY FileAnalysisService.Api/ FileAnalysisService.Api/
COPY FileAnalysisService.Application/ FileAnalysisService.Application/
COPY FileAnalysisService.Domain/ FileAnalysisService.Domain/
COPY FileAnalysisService.Infrastructure/ FileAnalysisService.Infrastructure/

# Сборка
WORKDIR /src/FileAnalysisService.Api
RUN dotnet build FileAnalysisService.Api.csproj -c $BUILD_CONFIGURATION -o /app/build

# Публикация
FROM build AS publish

RUN dotnet publish FileAnalysisService.Api.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# -------- Финальный образ --------
# FROM debian:bookworm-slim AS final
FROM ubuntu:22.04 AS final

ENV ASPNETCORE_HTTP_PORTS 8080
# ENV ASPNETCORE_HTTPS_PORTS 8081
EXPOSE 8080
# EXPOSE 8081


# Обновление и установка зависимостей
RUN apt-get update && apt-get install -y \
    curl \
    wget \
    ca-certificates \
    gnupg \
    && rm -rf /var/lib/apt/lists/*

# Установка ASP.NET Core Runtime 8
RUN wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb && \
    rm packages-microsoft-prod.deb && \
    apt-get update && \
    apt-get install -y aspnetcore-runtime-8.0 && \
    rm -rf /var/lib/apt/lists/*

# Установка OpenJDK 21 JRE (x64)
RUN curl -L -o openjdk21.tar.gz https://github.com/adoptium/temurin21-binaries/releases/download/jdk-21.0.7%2B6/OpenJDK21U-jre_x64_linux_hotspot_21.0.7_6.tar.gz && \
    mkdir -p /opt/jdk && \
    tar -xzf openjdk21.tar.gz --strip-components=1 -C /opt/jdk && \
    rm openjdk21.tar.gz


ENV JAVA_HOME=/opt/jdk
ENV PATH="${JAVA_HOME}/bin:${PATH}"

# Создание рабочих директорий
WORKDIR /app
COPY --from=publish /app/publish .

# Переменная окружения для пути к JAR
ARG JPLAG_FILE_LIB="/app/lib/jplag.jar"
ENV FAnalysis.COMPARISON_FILE=${JPLAG_FILE_LIB}

# Скачивание jplag.jar
RUN mkdir -p /app/lib && \
    curl -L -o ${JPLAG_FILE_LIB} https://github.com/jplag/JPlag/releases/download/v5.1.0/jplag-5.1.0-jar-with-dependencies.jar

# Копирование jplag.jar
# RUN mkdir -p /app/lib
# COPY ./jplag_5.1.jar ${JPLAG_FILE_LIB}


ENTRYPOINT ["dotnet", "FileAnalysisService.Api.dll"]
