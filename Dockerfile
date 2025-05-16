# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and restore as distinct layers
COPY GoonAuction.sln ./
COPY GoonAuctionAPI/GoonAuctionAPI.csproj GoonAuctionAPI/
COPY GoonAuctionBLL/GoonAuctionBLL.csproj GoonAuctionBLL/
COPY GoonAuctionDAL/GoonAuctionDAL.csproj GoonAuctionDAL/
RUN dotnet restore

# Copy everything else and build
COPY . .
WORKDIR /src/GoonAuctionAPI
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose port (default for ASP.NET Core)
EXPOSE 8080

# Set entrypoint
ENTRYPOINT ["dotnet", "GoonAuctionAPI.dll"] 