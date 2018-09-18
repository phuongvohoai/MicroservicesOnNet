FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 51060
EXPOSE 44332

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["src/Services/UserAuditLogs/WAL.UserLoginLogs.API/WAL.UserActivityLogs.API.csproj", "src/Services/UserAuditLogs/WAL.UserLoginLogs.API/"]
COPY ["src/Core/WebHost/WAL.WebHost/WAL.ServiceHost.csproj", "src/Core/WebHost/WAL.WebHost/"]
COPY ["src/Core/EventBus/WAL.EventBus.RabbitMQ/WAL.EventBus.RabbitMQ.csproj", "src/Core/EventBus/WAL.EventBus.RabbitMQ/"]
COPY ["src/Core/EventBus/WAL.EventBus/WAL.EventBus.csproj", "src/Core/EventBus/WAL.EventBus/"]
RUN dotnet restore "src/Services/UserAuditLogs/WAL.UserLoginLogs.API/WAL.UserActivityLogs.API.csproj"
COPY . .
WORKDIR "/src/src/Services/UserAuditLogs/WAL.UserLoginLogs.API"
RUN dotnet build "WAL.UserActivityLogs.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "WAL.UserActivityLogs.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WAL.UserActivityLogs.API.dll"]