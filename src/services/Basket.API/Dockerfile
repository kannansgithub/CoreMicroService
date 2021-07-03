#https://hub.docker.com/_/microsoft-dotnet-sdk/
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /src


# copy csproj and restore as distinct layers
COPY services/Basket.API/Basket.API.csproj services/Basket.API/
RUN dotnet restore "services/Basket.API/Basket.API.csproj"

# copy everything else and build app
COPY . .
WORKDIR /src/services/Basket.API/
RUN dotnet publish "Basket.API.csproj" -c Release -o /app/publish --no-restore

# final stage genarate runtime image
# https://hub.docker.com/_/microsoft-dotnet-aspnet/
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
EXPOSE 80
COPY --from=build-env /app/publish .
ENTRYPOINT ["dotnet", "Basket.API.dll"]