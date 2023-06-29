# MiniRestApp

MiniRestApp is a small .NET 6.0 solution consisting of two main projects: CsvProcessor and WebAPI.

## CsvProcessor

This is a console application that reads a CSV file containing customer data. The customer data includes the following fields:

- Customer Ref
- Customer Name
- Address Line 1
- Address Line 2
- Town
- County
- Country
- Postcode

The CSV file is read using the CsvHelper library, which takes care of parsing the CSV format and mapping the data to Customer objects.

Each Customer object is then serialized to JSON format and sent to a REST API endpoint (`api/customers`) using a POST request. The JSON serialization and the HTTP requests are handled by classes from the `System.Text.Json` and `System.Net.Http` namespaces, respectively.

The path of the CSV file is expected to be passed as a command line argument when running the CsvProcessor application.

## WebAPI

This is an ASP.NET Core 6.0 project that provides the REST API for managing customer data.

The API includes two main endpoints:

- **POST api/customers**: Accepts a Customer object in JSON format in the request body and saves it to a SQL Server database using Entity Framework Core. The Customer object is expected to include all the fields mentioned above.

- **GET api/customers/{id}**: Retrieves a Customer with the specified id from the database and returns it in JSON format.

The data is stored in a SQL Server database, and Entity Framework Core is used as the ORM (Object-Relational Mapper) for data access. The `CustomerContext` class is the `DbContext` subclass that represents the database session and can be used to query and save instances of the `Customer` class.

The `CustomersController` class contains the action methods for the POST and GET endpoints.

## Testing

The solution uses NUnit as the testing framework and follows a Test-Driven Development (TDD) approach. This means that tests were written for each feature before the actual implementation.

Tests were written to verify the correct parsing of the CSV file, the correct sending of HTTP requests, and the correct saving and retrieving of data in the database. The Moq library was used to mock dependencies for the tests, and the In-Memory Database provider for Entity Framework Core was used to simulate the database for testing purposes.
