# Library Management System API

## Overview

A RESTful API built with ASP.NET Core Web API for managing library operations such as book management, user authentication, borrowing and returning books, and fine calculations.

This project demonstrates clean backend architecture practices including JWT Authentication, Role-Based Authorization, Validation, Pagination, and Global Exception Handling.

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

---

## Technologies Used

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT Authentication
- FluentValidation
- Swagger / OpenAPI

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
Validators/
Middleware/
Migrations/
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

## API Documentation

Swagger UI will be available at:

```text
https://localhost:{port}/swagger
```

---

## Future Improvements

- Refresh Tokens
- Unit & Integration Testing
- Docker Support
- CI/CD Pipeline
- Caching
- Logging System

---

## Author

Mustafa Salah

GitHub:
https://github.com/MustafaSalah2001
