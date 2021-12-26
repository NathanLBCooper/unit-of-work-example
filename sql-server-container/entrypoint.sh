#!/usr/bin/env bash

echo 'running database setup'
./setup-database.sh & ./opt/mssql/bin/sqlservr
