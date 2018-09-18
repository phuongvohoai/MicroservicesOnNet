FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 51060
EXPOSE 44332

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["src/Services/UserAuditLogs/WAL.UserAuditLogs.API/WAL.UserAuditLogs.API.csproj", "src/Services/UserAuditLogs/WAL.UserAuditLogs.API/"]
RUN dotnet restore "src/Services/UserAuditLogs/WAL.UserAuditLogs.API/WAL.UserAuditLogs.API.csproj"
COPY . .
WORKDIR "/src/src/Services/UserAuditLogs/WAL.UserAuditLogs.API"
RUN dotnet build "WAL.UserAuditLogs.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "WAL.UserAuditLogs.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WAL.UserAuditLogs.API.dll"]