FROM mcr.microsoft.com/mssql/server:latest

USER root
RUN apt-get update && apt-get install -y dos2unix
USER mssql

ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=Password1!

COPY initialize-database.sql initialize-database.sql
COPY setup-database.sh setup-database.sh
COPY entrypoint.sh entrypoint.sh

USER root
# convert crlf -> lf 
RUN dos2unix ./initialize-database.sql && dos2unix ./setup-database.sh && dos2unix ./entrypoint.sh
# remove dos2unix and cleanup apt-get files
RUN apt-get --purge remove -y dos2unix && rm -rf /var/lib/apt/lists/
USER mssql

CMD /bin/bash ./entrypoint.sh
