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

# This Dockerfile is designed to containerize a .NET 8 web application using a multi-stage build process, which helps optimize the final image by reducing its size and improving performance. The process starts with the build stage, where the official .NET SDK 8.0 image is used as the base. A working directory (/app) is created inside the container, and the project file (*.csproj) is copied first. Then, the dotnet restore command retrieves and installs all required dependencies from NuGet. This step is done separately because Docker caches it, ensuring that dependencies don’t get downloaded again if they haven’t changed, significantly speeding up subsequent builds. Next, the remaining application files are copied, and dotnet publish compiles the application in Release mode, optimizing it for deployment. The compiled files are stored in the /out directory inside the container.

# Once the application is built, the second stage sets up the runtime environment. Instead of using the full SDK, it uses a much smaller ASP.NET Core runtime image, which contains only the essential components needed to run the application. The previously compiled files are copied from the build stage into this new runtime container. An environment variable is then set to ensure the application listens on port 5187 (ASPNETCORE_URLS=http://+:5187), allowing it to handle incoming requests on any network interface. The EXPOSE 5187 command informs Docker that the application will run on this port, though it does not automatically expose it to the host machine unless specified when running the container. Finally, the CMD ["dotnet", "RunClubAPI.dll"] command tells the container to launch the application when it starts by running the compiled RunClubAPI.dll file.

# The use of a multi-stage build in this Dockerfile offers several benefits. It ensures that the final image remains lightweight by excluding unnecessary files like source code and the full .NET SDK, keeping only the essential runtime components. It also makes builds faster, as restoring dependencies is cached, avoiding redundant downloads. Additionally, the separation between the build and runtime environments enhances security, as the final container does not include development tools or unnecessary dependencies. Overall, this Dockerfile provides an efficient, optimized, and secure way to deploy a .NET 8 web application using Docker.