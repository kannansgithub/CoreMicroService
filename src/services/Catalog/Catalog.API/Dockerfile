#https://hub.docker.com/_/microsoft-dotnet-sdk/
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /src

# copy csproj and restore as distinct layers
COPY services/Catalog/Catalog.API/Catalog.API.csproj services/Catalog/Catalog.API/
RUN dotnet restore "services/Catalog/Catalog.API/Catalog.API.csproj"

# copy everything else and build app
COPY . .
WORKDIR /src/services/Catalog/Catalog.API/
RUN dotnet publish "Catalog.API.csproj" -c Release -o /app/publish --no-restore

# final stage genarate runtime image
# https://hub.docker.com/_/microsoft-dotnet-aspnet/
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
EXPOSE 80
COPY --from=build-env /app/publish .
ENTRYPOINT ["dotnet", "Catalog.API.dll"]