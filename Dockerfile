# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy everything and restore dependencies
COPY . .  
RUN dotnet restore

# Build the application
RUN dotnet publish -c Release -o /out

# Stage 2: Runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .
ENV ASPNETCORE_URLS=http://+:5000
CMD ["dotnet", "RunClubAPI.dll"]
