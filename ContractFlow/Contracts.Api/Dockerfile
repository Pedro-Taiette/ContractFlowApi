FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Contracts.Api/Contracts.Api.csproj Contracts.Api/
COPY Contracts.Application/Contracts.Application.csproj Contracts.Application/
COPY Contracts.Infrastructure/Contracts.Infrastructure.csproj Contracts.Infrastructure/
COPY Contracts.Domain/Contracts.Domain.csproj Contracts.Domain/
RUN dotnet restore Contracts.Api/Contracts.Api.csproj

COPY . .
RUN dotnet publish Contracts.Api/Contracts.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Contracts.Api.dll"]
