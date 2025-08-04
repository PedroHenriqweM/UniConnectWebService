# Estágio 1: Build da aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SistemaAcademico/SistemaAcademico.Api/SistemaAcademico.Api.csproj", "SistemaAcademico/SistemaAcademico.Api/"]
COPY ["SistemaAcademico/SistemaAcademico.Application/SistemaAcademico.Application.csproj", "SistemaAcademico/SistemaAcademico.Application/"]
COPY ["SistemaAcademico/SistemaAcademico.Domain/SistemaAcademico.Domain.csproj", "SistemaAcademico/SistemaAcademico.Domain/"]
COPY ["SistemaAcademico/SistemaAcademico.Infrastructure/SistemaAcademico.Infrastructure.csproj", "SistemaAcademico/SistemaAcademico.Infrastructure/"]
RUN dotnet restore "SistemaAcademico/SistemaAcademico.Api/SistemaAcademico.Api.csproj"

# Copia o restante do código fonte
COPY . .
WORKDIR "/src/SistemaAcademico/SistemaAcademico.Api"
RUN dotnet build "SistemaAcademico.Api.csproj" -c Release -o /app/build

# Estágio 2: Publicação da aplicação
FROM build AS publish
RUN dotnet publish "SistemaAcademico.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Estágio 3: Execução da aplicação
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SistemaAcademico.Api.dll"]