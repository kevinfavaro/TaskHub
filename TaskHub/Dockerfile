# Imagem base para a aplica��o .NET
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Constru��o da aplica��o .NET
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TaskHub.csproj", "."]
RUN dotnet restore "./TaskHub.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./TaskHub.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publica��o da aplica��o
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TaskHub.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Imagem final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskHub.dll"]