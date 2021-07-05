#https://hub.docker.com/_/microsoft-dotnet-sdk/
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /src

# copy csproj and restore as distinct layers
COPY services/Discount/Discount.API/Discount.API.csproj services/Discount/Discount.API/
RUN dotnet restore "services/Discount/Discount.API/Discount.API.csproj"

# copy everything else and build app
COPY . .
WORKDIR /src/services/Discount/Discount.API/
RUN dotnet publish "Discount.API.csproj" -c Release -o /app/publish --no-restore

# final stage genarate runtime image
# https://hub.docker.com/_/microsoft-dotnet-aspnet/
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
EXPOSE 80
COPY --from=build-env /app/publish .
ENTRYPOINT ["dotnet", "Discount.API.dll"]