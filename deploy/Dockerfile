FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /repo

# copia todo o repositório (saindo da pasta deploy)
COPY .. .

# entra na pasta do projeto da API
WORKDIR /repo/src/ColinhoDaCaApi

# restaura e publica explicitamente o csproj
RUN dotnet restore ColinhoDaCaApi.csproj
RUN dotnet publish ColinhoDaCaApi.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "ColinhoDaCaApi.dll"]