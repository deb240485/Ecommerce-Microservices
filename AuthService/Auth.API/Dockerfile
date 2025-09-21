FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AuthService/Auth.API/Auth.API.csproj", "AuthService/Auth.API/"]
COPY ["AuthService/Auth.Application/Auth.Application.csproj", "AuthService/Auth.Application/"]
COPY ["AuthService/Auth.Core/Auth.Core.csproj", "AuthService/Auth.Core/"]
COPY ["AuthService/Auth.Infrastructure/Auth.Infrastructure.csproj", "AuthService/Auth.Infrastructure/"]
RUN dotnet restore "./AuthService/Auth.API/Auth.API.csproj"
COPY . .
WORKDIR "/src/AuthService/Auth.API"
RUN dotnet build "./Auth.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Auth.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

USER app
ENTRYPOINT ["dotnet", "Auth.API.dll"]