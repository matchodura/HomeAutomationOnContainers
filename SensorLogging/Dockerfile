#FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
#WORKDIR /app
#
#COPY *.csproj ./
#RUN dotnet restore
#
#COPY . ./
#RUN dotnet publish -c Release -o out
#
#FROM mcr.microsoft.com/dotnet/aspnet:5.0 
#WORKDIR /app
#COPY --from=build-env /app/out .
#ENTRYPOINT [ "dotnet", "SensorLogging.API.dll" ]
#
#python

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim-arm32v7
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet", "SensorLogging.API.dll" ]