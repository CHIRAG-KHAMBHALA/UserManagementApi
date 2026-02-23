# User Management API

A simple RESTful Web API built with **ASP.NET Core 8** for managing users. Built as a learning project to practice building APIs with proper structure, middleware, validation, and Swagger documentation.

## Tech Stack

- ASP.NET Core 8
- C#
- Swagger / Swashbuckle
- In-memory storage (no database)

## Project Structure

```
UserManagementAPI/
├── Controllers/        → API endpoints
├── DTOs/               → Request models with validation
├── Models/             → User entity
├── Data/               → In-memory user store
├── Middleware/         → Auth + request logging
├── Program.cs          → App setup and configuration
```

## Features

- Full CRUD — GET, POST, PUT, DELETE
- Custom **AuthMiddleware** — requires `X-API-KEY: dev-secret` for write operations
- Custom **RequestLoggingMiddleware** — logs method, path, status code, and response time
- Input validation using DataAnnotations
- Proper HTTP status codes (200, 201, 204, 400, 401, 404, 409)
- Swagger UI for testing endpoints
- Console logging for all key events

## API Endpoints

| Method | Endpoint | Auth Required | Description |
|--------|----------|---------------|-------------|
| GET | `/api/users` | No | Get all users |
| GET | `/api/users/{id}` | No | Get user by ID |
| POST | `/api/users` | Yes | Create a new user |
| PUT | `/api/users/{id}` | Yes | Update a user |
| DELETE | `/api/users/{id}` | Yes | Delete a user |

## Running the Project

```bash
git clone https://github.com/CHIRAG-KHAMBHALA/UserManagementApi.git
cd UserManagementApi/UserManagementAPI
dotnet run
```

Open Swagger UI: `http://localhost:5188/swagger`

## Authentication

POST, PUT, DELETE endpoints require the following header:

```
X-API-KEY: dev-secret
```

In Swagger, click the **Authorize** button (🔒) and enter `dev-secret`.

## Validation Rules

| Field | Rule |
|-------|------|
| FirstName | Required, max 100 chars |
| LastName | Required, max 100 chars |
| Email | Required, valid email format |
| Age | Between 0 and 120 |

## Sample Request

```json
POST /api/users
Content-Type: application/json
X-API-KEY: dev-secret

{
  "firstName": "abhay",
  "lastName": "sharma",
  "email": "asharma@example.com",
  "age": 28
}
```
