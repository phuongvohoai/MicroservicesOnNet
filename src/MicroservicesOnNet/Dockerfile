FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 52710
EXPOSE 44338

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["MicroservicesOnNet/MicroservicesOnNet.csproj", "MicroservicesOnNet/"]
RUN dotnet restore "MicroservicesOnNet/MicroservicesOnNet.csproj"
COPY . .
WORKDIR "/src/MicroservicesOnNet"
RUN dotnet build "MicroservicesOnNet.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MicroservicesOnNet.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MicroservicesOnNet.dll"]