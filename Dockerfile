# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY ContractFlowApi/ContractFlowApi.csproj ContractFlowApi/ContractFlowApi.csproj
RUN dotnet restore ContractFlowApi/ContractFlowApi.csproj

# Copy everything else and publish
COPY ContractFlowApi/. ContractFlowApi/
WORKDIR /src/ContractFlowApi
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "ContractFlowApi.dll"]
