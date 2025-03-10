# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0@sha256:032381bcea86fa0a408af5df63a930f1ff5b03116c940a7cd744d3b648e66749 \
    AS build-stage

WORKDIR /app

# Copia o arquivo da solução/projetos
COPY *.sln ./
COPY src/*.csproj ./src/
COPY tests/*.csproj ./tests/

# Restaura as dependências
RUN dotnet restore && \
    # Instala o dotnet-sonarscanner para integrar o projeto com o SonarQube
    dotnet tool install --global dotnet-sonarscanner && \
    # Instala o dotnet-ef para executar as migrations automaticamente na execução
    dotnet tool install --global dotnet-ef

# Adiciona o dotnet tools ao PATH
ENV PATH="$PATH:/root/.dotnet/tools"

# Copia o restante dos arquivos e publica a aplicação
COPY . ./
WORKDIR /app/src
RUN dotnet publish -c Release -o /app/out

# Etapa de produção
FROM mcr.microsoft.com/dotnet/aspnet:8.0@sha256:04f79dd7f0a27eb079c2498ca9bab729ed40b91ed393d24846d6cb505f3fe646 \
    AS production-stage

WORKDIR /app
COPY --from=build-stage /app/out .

# Executa as migrations antes de iniciar a aplicação
ENTRYPOINT ["dotnet", "Api.dll"]