# ASP.NET Core Backend Application "RestApp"

A backend application implementing the logic of a social network. It includes the following:
- **implementation of basic GET, POST, PUT, DELETE queries.
- **use of Entity Framework to communicate with MSSql database
- **error logger
- **registration, user login
- **implementation of user roles
- **user authentication using JWT token 
- **password hashing
- **password editing
- **query authorization with user role

It was written in ASP.NET Core.

## Features

- **JWT Authentication**: Secure API endpoints using JSON Web Tokens (JWT).
- **FluentValidation**: Built-in validation for data models and input parameters.
- **AutoMapper**: Object-to-object mapping for data transfer between layers.
- **Entity Framework Core**: ORM for interacting with SQL Server.
- **NLog**: Advanced logging system for debugging and monitoring.
- **Swagger**: Auto-generated API documentation.

## Prerequisites

Before running the application, make sure you have the following installed:

- .NET 6.0 SDK or later
- SQL Server (or any supported database engine)
- Visual Studio (or any preferred IDE for .NET development)

## Libraries Used

- **AutoMapper.Extensions.Microsoft.DependencyInjection**: Provides dependency injection integration for AutoMapper.
- **FluentValidation.AspNetCore**: Provides FluentValidation integration for ASP.NET Core.
- **Microsoft.AspNetCore.Authentication.JwtBearer**: Used for JWT authentication middleware.
- **Microsoft.EntityFrameworkCore**: Core Entity Framework functionalities.
- **Microsoft.EntityFrameworkCore.SqlServer**: SQL Server provider for Entity Framework Core.
- **Microsoft.EntityFrameworkCore.Tools**: Command-line tools for Entity Framework Core.
- **NLog.Web.AspNetCore**: Provides logging capabilities using NLog.
- **Swashbuckle.AspNetCore**: Enables Swagger integration for API documentation.

## Installation

To get started with the application, follow these steps:

1. Clone the repository:

   ```bash
   git clone https://github.com/your-username/your-repository-name.git
   ```
   
2. Navigate to the project directory:
   ```bash
    cd your-repository-name
   ```

3. Restore dependencies:
   ```bash
    dotnet restore
   ```
   
4.Set up the SQL Server database connection in the appsettings.json file:
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=your_server_name;Database=your_database_name;Trusted_Connection=True;"
    }
    ```
    
5.Apply migrations to set up the database:
   ```bash
    dotnet ef database update
   ```
6.Run the application:
   ```bash
    dotnet run
   ```
The application should now be running on https://localhost:5001 (default URL).

## Configuration
- **Logging**: NLog is used for logging. You can configure NLog in the nlog.config file located at the root of the project.
- **Swagger**: Swagger is enabled by default for API documentation. Access the API documentation at https://localhost:5001/swagger.

## Usage
- **Authentication**: Use JWT Bearer tokens for authentication. Pass the token in the Authorization header of your requests in the following format:

   ```makefile
    Authorization: Bearer <your-jwt-token>
   ```

- **FluentValidation**: All models are validated automatically by FluentValidation before they are processed by the application. If the validation fails, a response with a 400 Bad Request will be returned.

- **AutoMapper**: AutoMapper is used to map between your models and DTOs (Data Transfer Objects). You can define mappings in the MappingProfiles directory.

## API Endpoints
Check out the auto-generated Swagger documentation at https://localhost:5001/swagger for a full list of available API endpoints and how to use them.

## License
This project is licensed under the MIT License - see the LICENSE file for details.

Acknowledgments
- ASP.NET Core for providing a robust framework for building web applications.
- AutoMapper for simplifying object-to-object mapping.
- FluentValidation for making validation easier and more maintainable.
- NLog for offering an excellent logging system.
- Swashbuckle for providing an easy way to generate API documentation.

This README provides an overview of the project, its setup, dependencies, usage, and contribution guidelines. Make sure to adjust specific parts like the repository URL, and database connection details, and add any additional project-specific instructions as needed.
