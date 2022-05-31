#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CalendarService/HWA-GARDEN-CalendarService/HWA-GARDEN-CalendarService.csproj", "CalendarService/HWA-GARDEN-CalendarService/"]
COPY ["HWA-GARDEN.Utilities/HWA-GARDEN.Utilities.csproj", "HWA-GARDEN.Utilities/"]
COPY ["HWA-GARDEN.Contracts/HWA-GARDEN.Contracts.csproj", "HWA-GARDEN.Contracts/"]
COPY ["CalendarService/HWA-GARDEN-CalendarService.Domain/HWA-GARDEN-CalendarService.Domain.csproj", "CalendarService/HWA-GARDEN-CalendarService.Domain/"]
COPY ["CalendarService/HWA-GARDEN-CalendarService.Data/HWA-GARDEN-CalendarService.Data.csproj", "CalendarService/HWA-GARDEN-CalendarService.Data/"]
COPY ["HWA-GARDEN.Data.Utilities/HWA-GARDEN.Data.Utilities.csproj", "HWA-GARDEN.Data.Utilities/"]
COPY ["HWA-GARDEN.Data/HWA-GARDEN.Data.csproj", "HWA-GARDEN.Data/"]
RUN dotnet restore "CalendarService/HWA-GARDEN-CalendarService/HWA-GARDEN-CalendarService.csproj"
COPY . .
WORKDIR "/src/CalendarService/HWA-GARDEN-CalendarService"
RUN dotnet build "HWA-GARDEN-CalendarService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HWA-GARDEN-CalendarService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HWA-GARDEN-CalendarService.dll"]