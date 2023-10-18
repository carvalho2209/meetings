#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Meeting.Api/Meeting.Api.csproj", "src/Meeting.Api/"]
COPY ["src/Meeting.Application/Meeting.Application.csproj", "src/Meeting.Application/"]
COPY ["src/Meeting.Domain/Meeting.Domain.csproj", "src/Meeting.Domain/"]
COPY ["src/Meeting.Infrastructure/Meeting.Infrastructure.csproj", "src/Meeting.Infrastructure/"]
COPY ["src/Meeting.Persistence/Meeting.Persistence.csproj", "src/Meeting.Persistence/"]
RUN dotnet restore "./src/Meeting.Api/./Meeting.Api.csproj"
COPY . .
WORKDIR "/src/src/Meeting.Api"
RUN dotnet build "./Meeting.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Meeting.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Meeting.Api.dll"]