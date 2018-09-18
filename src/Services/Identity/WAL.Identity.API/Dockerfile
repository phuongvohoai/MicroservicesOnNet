FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 52774
EXPOSE 44377

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["src/Services/Identity/WAL.Identity.API/WAL.Identity.API.csproj", "src/Services/Identity/WAL.Identity.API/"]
RUN dotnet restore "src/Services/Identity/WAL.Identity.API/WAL.Identity.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Identity/WAL.Identity.API"
RUN dotnet build "WAL.Identity.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "WAL.Identity.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WAL.Identity.API.dll"]