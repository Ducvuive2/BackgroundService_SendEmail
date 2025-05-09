# Employee Report Service

This is an ASP.NET Core application that automatically sends employee reports via email every 15 minutes.

## Features

- Clean Architecture design with 4 layers: Domain, Application, Infrastructure, and Web API
- Automatic email sending with Excel report attachment every 15 minutes
- Uses Entity Framework Core with SQL Server for data access from AdventureWorks database
- EPPlus for Excel file generation
- MailKit for email sending

## Project Structure

- **Domain**: Contains the Employee entity and business logic
- **Application**: Contains interfaces and use cases (EmployeeReportService)
- **Infrastructure**: Contains implementations for database access, Excel generation, and email sending
- **WebAPI**: Hosts the application and runs the background service

## Database Structure

This application works with the AdventureWorks SQL Server database, specifically querying the HumanResources.Employee table with the following query:

```sql
SELECT *
FROM humanresources.employee
ORDER BY jobtitle;
```

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server with the AdventureWorks database installed
- An email account for sending emails (Gmail configured)

### Configuration

1. The connection string in `appsettings.json` is configured for AdventureWorks database:

```json
"ConnectionStrings": {
  "AdventureWorksConnection": "Server=MSI;Database=AdventureWorks;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

2. Email settings are configured in `appsettings.json`:

```json
"MailSettings": {
  "SenderName": "Employee Report Bot",
  "SenderEmail": "duc20062001@gmail.com",
  "Username": "duc20062001@gmail.com",
  "Password": "",
  "Server": "smtp.gmail.com",
  "Port": 587
}
```

### Running the Application

```bash
cd WebAPI
dotnet run
```

Access the Swagger UI at: https://localhost:7120/swagger

## How It Works

1. The application starts and registers a background service (`EmployeeEmailBackgroundService`)
2. Every 15 minutes, the service:
   - Retrieves employee data from the HumanResources.Employee table ordered by JobTitle
   - Generates an Excel report with all employee fields
   - Sends an email with the attached report to `duc2006200101@gmail.com` using MailKit

## API Endpoints

- GET /api/employees - Returns a list of employees ordered by job title
