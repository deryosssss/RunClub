# RunClub Web Application

## 1. Project Title

**RunClub** –  A backend service for managing running club memberships, enrollments, and progress tracking.

## 2. Project Description

RunClub API is a RESTful web service built with ASP.NET Core and Entity Framework Core. It provides functionality for users to register, enroll in running events, track their progress, and manage their roles within the application. The API is designed with JWT authentication for security and follows best practices for logging, error handling, and performance optimization. RunClub is a platform that connects runners, organizes running events, and tracks progress. Users can join local running groups, share achievements, and participate in challenges. The platform aims to build a supportive and motivating environment for runners of all levels.

## 3. Features

- User Management: Allows users to register, login, and manage authentication via JWT.

- Event Enrollment: Users can enroll in different running events and track their participation.

- Progress Tracking: Users can log their running progress and view historical data.

- Role-Based Access Control (RBAC): Admins can manage events and users, while regular users can enroll in events and track progress.

- Pagination & Filtering: Efficient data retrieval with built-in pagination and filtering.

- Security: Implements JWT authentication and IP rate limiting to prevent unauthorized access.

- Health Monitoring: A /health endpoint for checking the API's operational status.

##  4. Installation Instructions

1. Clone the repository

To get started, clone the repository to your local machine:

[RunClub GitHub Repository](https://github.com/deryosssss/RunClub.git)
cd runclub-api

2. Install dependencies

Ensure you have the .NET SDK installed. Then, restore dependencies using:

dotnet restore

3. Set up the database

This project uses Entity Framework Core for database management. Apply migrations to set up the database:

dotnet ef database update

4. Set up environment variables

Create an .env file in the root directory and define the following variables:

ConnectionStrings__RunClubDb="YourDatabaseConnectionString"
Jwt__Key="YourSuperSecretKey"
Jwt__Issuer="https://yourapi.com"
AllowedOrigins="http://localhost:5187"

⚠️ Security Notice: Never commit your .env file or expose secrets in a public repository. Use a secrets manager like Azure Key Vault in production.

5. Start the server

Run the application locally using:

dotnet run

The API will be accessible at:
http://localhost:5187

## 5. Running the Application in Production (Azure)

This project is deployed on Azure. You can access the live application here:
[RunClub API on Azure](https://myrunclub.azurewebsites.net)

Health Check

To monitor the API's status, visit:
[RunClub API on Azure](https://myrunclub.azurewebsites.net/health)

Logs and Monitoring

Application logs and error tracking can be viewed in Azure Monitor.

For deeper insights, enable Application Insights for performance monitoring and debugging.

## 6. Running Tests

To ensure the API functions correctly, run the test suite using:

dotnet test

Test Coverage

Unit Tests: Verify individual components.

Integration Tests: Validate API endpoint behavior.

## 7. API Documentation

This project includes built-in API documentation using Swagger. You can access the API documentation locally at:
[RunClub API on Swagger](http://localhost:5187/swagger)

For production, access it at:
https://runclub-api.azurewebsites.net/swagger


## 8. Contributing

Contributions are welcome! If you'd like to improve the project, follow these steps:

Fork the repository.

Create a new feature branch.

Make your changes and commit them.

Submit a pull request.

For major changes, please open an issue first to discuss the proposal.

## 9. Contact

For any questions or support, reach out to:

GitHub Issues: https://github.com/deryosss/runclub/issues

Thank you for using RunClub API! 