# Library Management System API

![.NET](https://img.shields.io/badge/.NET-8.0-blue)
![Tests](https://img.shields.io/badge/tests-passing-brightgreen)
![CI](https://img.shields.io/badge/CI-passing-success)
![License](https://img.shields.io/badge/license-MIT-green)

---

## Overview

A RESTful API built with ASP.NET Core Web API for managing library operations such as:

- Book management
- User authentication & authorization
- Borrowing and returning books
- Fine calculation system
- Role-based access control

This project demonstrates backend development best practices including JWT Authentication, Validation, Exception Handling, Unit Testing, and CI automation.

---

## Features

- JWT Authentication & Authorization
- Role-Based Access Control
- CRUD Operations for Books
- Borrow & Return Books
- Fine Calculation System
- Pagination & Filtering
- FluentValidation
- Global Exception Middleware
- Swagger API Documentation
- Entity Framework Core with SQL Server
- Unit Testing with xUnit
- GitHub Actions CI Pipeline

---

## Technologies Used

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT Authentication
- FluentValidation
- Swagger / OpenAPI
- xUnit
- GitHub Actions

---

## Roles & Permissions

| Role | Permissions |
|------|-------------|
| Admin | Full access |
| Librarian | Manage books and borrowings |
| Member | Borrow and return books |

---

## Project Structure

```text
Controllers/
DTOs/
Models/
Data/
Services/
Validators/
Middleware/
Migrations/
Tests/
```

---

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/MustafaSalah2001/library-management-api.git
```

### 2. Navigate to the project directory

```bash
cd library-management-api
```

### 3. Update the database

```bash
dotnet ef database update
```

### 4. Run the application

```bash
dotnet run
```

---

## Authentication

This API uses JWT Authentication.

Add the token in Swagger using:

```text
Bearer YOUR_TOKEN
```

---

## Running Tests

```bash
dotnet test
```

---

## Continuous Integration

GitHub Actions automatically:

- Restores dependencies
- Builds the project
- Runs all unit tests on every push

---

## API Documentation

Swagger UI will be available at:

```text
https://localhost:{port}/swagger
```

---

## Future Improvements

- Refresh Tokens
- Docker Support
- Integration Testing
- Deployment

---

## Author

Mustafa Salah

GitHub:
https://github.com/MustafaSalah2001
