#!/bin/bash

set -e

# Расщепление переменной окружения
IFS=',' read -ra DBS <<< "$POSTGRES_MULTIPLE_DATABASES"

# Создание баз данных от имени postgres
for db in "${DBS[@]}"; do
  echo "Creating database: $db"
  psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" <<-EOSQL
    CREATE DATABASE "$db";
EOSQL
done