# Etapa 1 — Build da aplicação
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia os arquivos de projeto (melhor cache)
COPY ReteSocialis.sln ./
COPY ReteSocialis.API/*.csproj ReteSocialis.API/
RUN dotnet restore ReteSocialis.API/ReteSocialis.API.csproj

# Copia o restante do código e publica
COPY . .
WORKDIR /src/ReteSocialis.API
RUN dotnet publish -c Release -o /app/publish

# Etapa 2 — Imagem final
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "ReteSocialis.API.dll"]
