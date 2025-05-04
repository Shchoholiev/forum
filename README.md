# forum

Web API for a forum application utilizing MongoDB, designed with clean architecture principles and built on ASP.NET Core.

## Table of Contents

- [Features](#features)
- [Stack](#stack)
- [Installation](#installation)
  - [Prerequisites](#prerequisites)
  - [Setup Instructions](#setup-instructions)
- [Configuration](#configuration)
- [Usage](#usage)

## Features

- **User Management**: Register, authenticate, and manage user profiles.
- **Thread Creation**: Users can create and manage discussion threads.
- **Commenting System**: Post and manage comments within threads.
- **Search Functionality**: Search for threads and comments.
- **Moderation Tools**: Admin capabilities for content moderation.

## Stack

- **Backend**: ASP.NET Core 6.0
- **Database**: MongoDB
- **Architecture**: Clean Architecture
- **ORM**: MongoDB Driver for .NET

## Installation

### Prerequisites

- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [MongoDB](https://www.mongodb.com/try/download/community)

### Setup Instructions

1. **Clone the Repository**:

   ```bash
   git clone https://github.com/Shchoholiev/forum.git
   cd forum
   ```

2. **Restore Dependencies**:

   ```bash
   dotnet restore
   ```

3. **Build the Project**:

   ```bash
   dotnet build
   ```

4. **Run the Application**:

   ```bash
   dotnet run
   ```

## Configuration

Configure the application settings in the `appsettings.json`.

- **MongoDb**: Connection string for MongoDB.
- **JwtSettings**: Settings for JWT authentication.

## Usage

- **Register a New User**: Access the `/api/auth/register` endpoint to create a new user account.
- **Authenticate**: Use the `/api/auth/login` endpoint to obtain a JWT token.
- **Create a Thread**: Send a POST request to `/api/threads` with the thread details.
- **Post a Comment**: Use the `/api/comments` endpoint to add comments to threads.
- **Search**: Utilize the `/api/search` endpoint to find threads and comments.
- **Moderation**: Admin users can access `/api/admin` endpoints for content moderation tasks.
