# https://hub.docker.com/_/microsoft-dotnet-aspnet/
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

# https://hub.docker.com/_/microsoft-dotnet-sdk/
FROM mcr.microsoft.com/dotnet/sdk:5.0  AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY services/Catalog.API/Catalog.API.csproj services/Catalog.API/
RUN dotnet restore "services/Catalog.API/Catalog.API.csproj"

WORKDIR /src/services/Catalog.API/
# copy everything else and build app
COPY . .
RUN dotnet build "Catalog.API.csproj" -c Release -o app/build


FROM build AS publish
RUN dotnet publish "Catalog.API.csproj" -c Release -o /app/publish --no-restore

# final stage genarate runtime image
FROM base AS final
WORKDIR /app
EXPOSE 80
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.API.dll"]



# https://hub.docker.com/_/microsoft-dotnet-sdk/
# FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
# WORKDIR /source

# # copy csproj and restore as distinct layers
# COPY services/Catalog.API/Catalog.API.csproj ./
# RUN dotnet restore

# # copy everything else and build app
# COPY . .
# RUN dotnet publish "Catalog.API.csproj" -c release -o /out --no-restore

# # final stage genarate runtime image
# # https://hub.docker.com/_/microsoft-dotnet-aspnet/
# FROM mcr.microsoft.com/dotnet/aspnet:5.0
# WORKDIR /app
# EXPOSE 80
# COPY --from=build-env /app/out .
# ENTRYPOINT ["dotnet", "Catalog.API.dll"]