FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /app
EXPOSE 80

# Copy all .csproj files and restore as distinct layer . Use the same
# copy command for every docker file in the project to take advantage of docker cashing
COPY Carsties.sln Carsties.sln
COPY src/AuctionService/AuctionService.csproj src/AuctionService/AuctionService.csproj
COPY src/Contracts/Contracts.csproj src/Contracts/Contracts.csproj 
COPY src/SearchService/SearchService.csproj src/SearchService/SearchService.csproj
COPY src/IdentityService/IdentityService.csproj src/IdentityService/IdentityService.csproj
COPY src/GatewayService/GatewayService.csproj src/GatewayService/GatewayService.csproj

# Restore packages
RUN dotnet restore Carsties.sln

# Copy app folder
COPY src/IdentityService src/IdentityService
WORKDIR /app/src/IdentityService
RUN dotnet publish -c Release -o /app/src/out

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/src/out .
ENTRYPOINT ["dotnet","IdentityService.dll"]

