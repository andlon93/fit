#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["FitnessTracker/FitnessTracker.csproj", "FitnessTracker/"]
RUN dotnet restore "FitnessTracker/FitnessTracker.csproj"
COPY . .
WORKDIR "/src/FitnessTracker"
RUN dotnet build "FitnessTracker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FitnessTracker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FitnessTracker.dll"]