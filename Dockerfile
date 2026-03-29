# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and restore as distinct layers
COPY LuveAPI.sln ./
COPY LuveAPI/LuveAPI.csproj LuveAPI/
COPY LuveBLL/LuveBLL.csproj LuveBLL/
COPY LuveDAL/LuveDAL.csproj LuveDAL/
COPY LuveAPI.Tests/LuveAPI.Tests.csproj LuveAPI.Tests/
COPY LuveBLL.Tests/LuveBLL.Tests.csproj LuveBLL.Tests/
RUN dotnet restore

# Copy everything else and build
COPY . .
WORKDIR /src/LuveAPI
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose port (default for ASP.NET Core)
EXPOSE 8080

# Set entrypoint
ENTRYPOINT ["dotnet", "LuveAPI.dll"] 