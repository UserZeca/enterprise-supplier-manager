# Estágio de Execução (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Estágio de Compilação (Build)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /source

# Copia todos os arquivos .csproj
COPY ["src/backend/EnterpriseSupplierManager.Api/EnterpriseSupplierManager.Api.csproj", "src/backend/EnterpriseSupplierManager.Api/"]
COPY ["src/backend/EnterpriseSupplierManager.Application/EnterpriseSupplierManager.Application.csproj", "src/backend/EnterpriseSupplierManager.Application/"]
COPY ["src/backend/EnterpriseSupplierManager.Domain/EnterpriseSupplierManager.Domain.csproj", "src/backend/EnterpriseSupplierManager.Domain/"]
COPY ["src/backend/EnterpriseSupplierManager.Infrastructure/EnterpriseSupplierManager.Infrastructure.csproj", "src/backend/EnterpriseSupplierManager.Infrastructure/"]

# Restaura as dependências
RUN dotnet restore "src/backend/EnterpriseSupplierManager.Api/EnterpriseSupplierManager.Api.csproj"

# Copia todo o código fonte e compila
COPY . .
WORKDIR "/source/src/backend/EnterpriseSupplierManager.Api"
RUN dotnet build "EnterpriseSupplierManager.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Estágio de Publicação
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "EnterpriseSupplierManager.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Estágio Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EnterpriseSupplierManager.Api.dll"]