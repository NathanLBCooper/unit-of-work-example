#!/usr/bin/env bash
echo 'Waiting for sql server to start (60s)'
sleep 60s

echo 'Initializing database'
./opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Password1! -d master -i initialize-database.sql

echo 'Finished initializing database'
