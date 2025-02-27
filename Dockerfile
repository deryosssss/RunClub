# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy project files and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the application and build
COPY . .
RUN dotnet publish -c Release -o /out --no-restore

# Stage 2: Runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy built application from build stage
COPY --from=build /out .

# Set environment variable for ASP.NET Core
ENV ASPNETCORE_URLS=http://+:5187

# Expose the application port
EXPOSE 5187

# Start the application
CMD ["dotnet", "RunClubAPI.dll"]
