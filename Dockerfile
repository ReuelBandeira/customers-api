# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-stage
WORKDIR /app

# Copia apenas os arquivos de projeto primeiro para melhor uso de cache
COPY *.sln ./
COPY src/*.csproj ./src/
COPY tests/*.csproj ./tests/

# Restaura dependências (etapa em cache se os projetos não mudarem)
RUN dotnet restore

# Copia e publica
COPY . ./
WORKDIR /app/src
RUN dotnet publish -c Release -o /app/out

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copia apenas os binários publicados
COPY --from=build-stage /app/out .

# Configurar o ponto de entrada
ENTRYPOINT ["dotnet", "Api.dll"]