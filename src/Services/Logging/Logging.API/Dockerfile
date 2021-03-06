#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim-arm64v8 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim-arm64v8 AS build
WORKDIR /src
COPY ["src/Services/Logging/Logging.API/Logging.API.csproj", "src/Services/Logging/Logging.API/"]
COPY ["src/Common/Entities/Entities.csproj", "src/Common/Entities/"]
RUN dotnet restore "src/Services/Logging/Logging.API/Logging.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Logging/Logging.API"
RUN dotnet build "Logging.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Logging.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Logging.API.dll"]