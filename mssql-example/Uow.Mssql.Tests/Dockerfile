#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["mssql-example/Uow.Mssql.Tests/Uow.Mssql.Tests.csproj", "mssql-example/Uow.Mssql.Tests/"]
COPY ["mssql-example/Uow.Mssql.Database/Uow.Mssql.Database.csproj", "mssql-example/Uow.Mssql.Database/"]
COPY ["mssql-example/Uow.Mssql.Migrator/Uow.Mssql.Migrator.csproj", "mssql-example/Uow.Mssql.Migrator/"]
RUN dotnet restore "mssql-example/Uow.Mssql.Tests/Uow.Mssql.Tests.csproj"
COPY . .
WORKDIR "/src/mssql-example/Uow.Mssql.Tests"
RUN dotnet build "Uow.Mssql.Tests.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Uow.Mssql.Tests.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Uow.Mssql.Tests.dll"]
