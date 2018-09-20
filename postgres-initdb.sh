#!/bin/sh -e

psql --variable=ON_ERROR_STOP=1 --username "postgres" <<-EOSQL
    CREATE ROLE root WITH LOGIN PASSWORD 'root';
    CREATE DATABASE "UserActivityLogsAPI" OWNER = root;
    GRANT ALL PRIVILEGES ON DATABASE "UserActivityLogsAPI" TO root;
EOSQL