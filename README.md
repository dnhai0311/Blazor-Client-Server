
# Blazor Client-Server App

This is a **Blazor** client-server application using Entity Framework Core for data management and **JWT authentication** for securing the API. The server uses `DbContext` to map to a MySQL database, and JWT tokens for user authentication. Every time you clone or pull the repository, you'll need to run the `update-database` command to ensure the database is synchronized with the current models.

## Features

- **Blazor WebAssembly** for the client-side.
- **ASP.NET Core** for the server-side.
- **Entity Framework Core** for database operations.
- **JWT authentication** for secure user sessions with access tokens and refresh tokens.
- **MySQL** as the database with XAMPP setup.
- Supports automatic database migrations with the `update-database` command.

## Getting Started

### Prerequisites

Ensure you have the following installed:

- [.NET 7.0 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) or [VS Code](https://code.visualstudio.com/)
- [XAMPP](https://www.apachefriends.org/index.html) with MySQL configured

### Installation

1. **Clone the repository**:

   ```bash
   git clone https://github.com/dnhai0311/Blazor-Client-Server.git
   cd Blazor-Client-Server
   ```

2. **Navigate to the server project**:

   ```bash
   cd Server
   ```

3. **Restore the required packages**:

   ```bash
   dotnet restore
   ```

4. **Configure your MySQL connection**:

   Open `appsettings.json` and update the `ConnectionStrings` to point to your MySQL database hosted on XAMPP:

   ```json
   "ConnectionStrings": {
     "DBConnection": "Server=localhost;Database=razor_book_manager;User=root;Password=;"
   }
   ```

5. **Update the database**:

   Run the following command to apply the latest migrations and update the database:

   ```bash
   dotnet ef database update
   ```

### JWT Authentication Setup

This project uses **JWT (JSON Web Token)** for user authentication. Here is the specific JWT configuration used in the project:

```json
"Jwt": {
  "Issuer": "https://localhost:7103",
  "Audience": "https://localhost:7103",
  "Key": "@@@@@@----duongngochaib2003831----@@@@@@",
  "RefreshKey": "@@@@@@----duongngochaib2003831refreshtoken----@@@@@@",
  "TokenExpiry": 30, // Access token expiry in minutes (30 minutes)
  "RefreshTokenExpiry": 7 // Refresh token expiry in days (7 days)
}
```

- **Issuer**: The URL of the token issuer, which is the server's local address.
- **Audience**: The intended audience of the token, which is also the server's local address.
- **Key**: The secret key used to sign the JWT access tokens.
- **RefreshKey**: The secret key used to sign the refresh tokens.
- **TokenExpiry**: The access token expiry time is set to 30 minutes.
- **RefreshTokenExpiry**: The refresh token expiry time is set to 7 days.

### Running the Application

1. **Start XAMPP**:
   - Ensure MySQL is running in XAMPP.

2. **Start the server**:

   ```bash
   dotnet run --project Server
   ```

3. **Start the client**:

   If you are using Visual Studio, press F5. If you are using the terminal:

   ```bash
   dotnet run --project Client
   ```

4. Open your browser and navigate to `https://localhost:5001`.


### Database Migrations

If you make changes to your models, generate new migrations by running:

```bash
dotnet ef migrations add MigrationName
```

Then update the database with:

```bash
dotnet ef database update
```

### Technologies Used

- **Blazor WebAssembly** - For the client-side UI.
- **ASP.NET Core** - For the server-side API.
- **Entity Framework Core** - For interacting with the MySQL database.
- **JWT (JSON Web Token)** - For securing the API with user authentication (access and refresh tokens).
- **MySQL (XAMPP)** - As the database system.
