# Overview

This is a ASP.NET Web API that support query and mutations of New Zealand Walks information.

Tools: C#, ASP.NET CORE, .NET8, Docker, Microsoft SQL Server, Azure Data Studio, Entity Framework Core, AWS (EC2, ECR)

# User Auth API

## Base URL

`/api/Auth`

## Endpoints

### Register User

**POST** `/api/Auth/Register`

#### Request Body

- `registerRequestDto` (RegisterRequestDto, required):
  ```json
  {
    "username": "string",
    "password": "string",
    "roles": ["string"]
  }
  ```

#### Responses

- 200 OK: User registered successfully.

```json
"User registered successfully."
```

- 400 Bad Request: Something went wrong during registration.

```json
"Something went wrong!"
```

### Login User

**POST** /api/Auth/Login

#### Request Body

- loginRequestDto (LoginRequestDto, required):

```json
{
  "username": "string",
  "password": "string"
}
```

#### Responses

- 200 OK: Login successful, returns a JWT token.

```json
{
  "jwtToken": "string"
}
```

- 400 Bad Request: Login attempt failed.

```json
"Login attempt failed."
```

# Walks API Documentation

## Base URL

`/api/v1.0/walks`

## Endpoints

### Get All Walks

**GET** `/api/v1.0/walks`

#### Query Parameters

- `filterBy` (string, optional): The field to filter by (e.g., "Name").
- `filterQuery` (string, optional): The filter query (e.g., "Park").
- `orderBy` (string, optional): The field to order by.
- `isAscending` (boolean, optional): Order in ascending or descending. Default is true.
- `pageNum` (int, optional): Page number. Default is 1.
- `pageSize` (int, optional): Page size. Default is 1000.

#### Responses

- **200 OK**: Returns a list of walks.
  ```json
  [
    {
      "id": "guid",
      "name": "string",
      "description": "string",
      "lengthInKm": "double",
      "regionId": "guid",
      "walkDifficultyId": "guid"
    }
  ]
  ```

### Get Walk By ID

**GET** `/api/v1.0/walks/{id}`

#### Path Parameters

- id (Guid, required): The ID of the walk.

#### Responses

- 200 OK: Returns the walk with the specified ID.
  ```json
  {
    "id": "guid",
    "name": "string",
    "description": "string",
    "lengthInKm": "double",
    "regionId": "guid",
    "walkDifficultyId": "guid"
  }
  ```
- 404 Not Found: Walk not found.

### Create Walk

**POST** /api/v1.0/walks

#### Request Body

- addWalkRequestDto (AddWalkRequestDto, required):

```json
{
  "name": "string",
  "description": "string",
  "lengthInKm": "double",
  "regionId": "guid",
  "walkDifficultyId": "guid"
}
```

#### Responses

- 201 Created: Returns the created walk.

```json
{
  "id": "guid",
  "name": "string",
  "description": "string",
  "lengthInKm": "double",
  "regionId": "guid",
  "walkDifficultyId": "guid"
}
```

- 400 Bad Request: Invalid request body.

### Update Walk

**PUT** /api/v1.0/walks/{id}

#### Path Parameters

- id (Guid, required): The ID of the walk to update.

#### Request Body

updateWalkRequestDto (UpdateWalkRequestDto, required):

```json
{
  "name": "string",
  "description": "string",
  "lengthInKm": "double",
  "regionId": "guid",
  "walkDifficultyId": "guid"
}
```

#### Responses

- 200 OK: Returns the updated walk.

```json
{
  "id": "guid",
  "name": "string",
  "description": "string",
  "lengthInKm": "double",
  "regionId": "guid",
  "walkDifficultyId": "guid"
}
```

- 404 Not Found: Walk not found.
- 400 Bad Request: Invalid request body.
