#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["EventService/HWA-GARDEN-EventService/HWA-GARDEN-EventService.csproj", "EventService/HWA-GARDEN-EventService/"]
COPY ["EventService/EventService.Domain/HWA-GARDEN-EventService.Domain.csproj", "EventService/EventService.Domain/"]
COPY ["EventService/EventService.DataAccess/HWA-GARDEN-EventService.Data.csproj", "EventService/EventService.DataAccess/"]
COPY ["HWA-GARDEN.Data.Utilities/HWA-GARDEN.Data.Utilities.csproj", "HWA-GARDEN.Data.Utilities/"]
COPY ["HWA-GARDEN.Utilities/HWA-GARDEN.Utilities.csproj", "HWA-GARDEN.Utilities/"]
COPY ["HWA-GARDEN.Data/HWA-GARDEN.Data.csproj", "HWA-GARDEN.Data/"]
COPY ["HWA-GARDEN.Contracts/HWA-GARDEN.Contracts.csproj", "HWA-GARDEN.Contracts/"]
RUN dotnet restore "EventService/HWA-GARDEN-EventService/HWA-GARDEN-EventService.csproj"
COPY . .
WORKDIR "/src/EventService/HWA-GARDEN-EventService"
RUN dotnet build "HWA-GARDEN-EventService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HWA-GARDEN-EventService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HWA-GARDEN-EventService.dll"]