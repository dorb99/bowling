# Bowling Scoreboard API

A RESTful API for managing bowling games, calculating scores, and tracking high scores. Built with ASP.NET Core 8 and Entity Framework Core

## 🎯 Features

- ✅ Create and manage bowling games
- ✅ Process bowling rolls with comprehensive validation
- ✅ Automatic strike and spare score calculation
- ✅ Bonus roll handling (10th frame)
- ✅ High score tracking with automatic top 5 management
- ✅ Entity Framework Core with SQL Server
- ✅ Swagger/OpenAPI documentation
- ✅ Clean architecture with separation of concerns
- ✅ Comprehensive validation service

## 📋 Prerequisites

- .NET 8.0 SDK
- SQL Server (LocalDB or full installation)
- Visual Studio 2022 or VS Code (optional)

## 🚀 Setup

### 1. Update Connection String

Edit `appsettings.json` with your SQL Server connection:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=BowlingDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 2. Apply Database Migrations

```bash
dotnet ef database update
```

Or from Visual Studio Package Manager Console:

```powershell
Update-Database
```

### 3. Run the Application

```bash
cd bowlingApp
dotnet run
```

Or press F5 in Visual Studio.

### 4. Access Swagger UI

Navigate to: `https://localhost:7151/swagger`

## 🏗️ Architecture

### Project Structure

```
bowlingApp/
├── Constants/
│   └── BowlingConstants.cs          # Game rules constants and validation messages
├── Controllers/
│   └── BowlingGameController.cs     # API endpoints
├── Data/
│   └── ApplicationDbContext.cs      # EF Core DbContext
├── Migrations/                      # EF Core migrations
├── Models/
│   ├── Dto/
│   │   └── StartGameRequest.cs      # Data Transfer Objects
│   ├── Frame.cs                     # Frame entity
│   ├── Game.cs                      # Game entity
│   ├── HighScore.cs                 # HighScore entity
│   ├── RollInput.cs                 # Roll input model
│   └── TurnResult.cs                # Turn result response
├── Repository/
│   ├── IBowlingRepository.cs        # Repository interface
│   └── BowlingRepository.cs         # Data access implementation
├── Services/
│   ├── IBowlingGameService.cs       # Business logic interface
│   ├── BowlingGameService.cs        # Core game logic
│   ├── IBowlingValidationService.cs # Validation interface
│   └── BowlingValidationService.cs  # Input validation logic
└── Program.cs                       # Application startup

```

### Architecture Layers

#### **Controllers**
- Handle HTTP requests and responses
- Perform basic input validation
- Return appropriate status codes

#### **Services**
- **BowlingGameService**: Core business logic, score calculation, and game rules
- **BowlingValidationService**: Validates roll inputs according to bowling rules

#### **Repository**
- Data access layer
- Entity Framework Core operations
- Database queries and updates

#### **Models**
- Domain entities (Game, Frame, HighScore)
- DTOs for API requests/responses
- Input/output models

#### **Constants**
- Centralized game rules constants
- Validation error messages
- Improves maintainability

## 📡 API Endpoints

### Start New Game

```http
POST /api/bowling/start
Content-Type: application/json

{
  "gameName": "Player Name"
}
```

**Response:**
```json
{
  "id": 1,
  "name": "Player Name",
  "frames": [],
  "score": 0,
  "currentFrameNumber": 0,
  "isGameOver": false
}
```

### Submit Turn/Frame

```http
POST /api/bowling/turn
Content-Type: application/json

{
  "gameId": 1,
  "roll1": 7,
  "roll2": 3,
  "roll3": null
}
```

**Response:**
```json
{
  "isSuccess": true,
  "errorMessage": null,
  "state": {
    "id": 1,
    "name": "Player Name",
    "frames": [...],
    "score": 10,
    "currentFrameNumber": 1,
    "isGameOver": false
  }
}
```

### Get Game by ID

```http
GET /api/bowling/{gameId}
```

**Response:** Game object with all frames and current score.

### Get High Scores

```http
GET /api/bowling/highscores
```

**Response:**
```json
[
  {
    "id": 1,
    "name": "Player Name",
    "score": 300,
    "dateAchieved": "2024-12-02T10:30:00Z"
  }
]
```

## 🎳 Bowling Rules Implemented

### Standard Frames (1-9)
- **Two rolls maximum** per frame
- **Strike**: All 10 pins knocked down on first roll (Roll2 must be null)
- **Spare**: All 10 pins knocked down in two rolls
- **Open Frame**: Less than 10 pins knocked down in two rolls

### 10th Frame (Final Frame)
- **Three rolls allowed** if strike or spare is achieved
- **Two rolls** for open frame
- **Bonus rolls** count toward the frame score

### Scoring Rules
- **Strike**: 10 + next two rolls
- **Spare**: 10 + next roll
- **Open Frame**: Sum of pins knocked down
- **Perfect Game**: 300 (12 consecutive strikes)
- **Cumulative scoring** across all frames

### Validation Rules
1. **Pin Count**: Each roll must be 0-10 pins
2. **Frame Total**: Regular frames cannot exceed 10 pins (except strikes)
3. **Roll Requirements**:
   - Strike: Roll1 = 10, Roll2 = null
   - Regular Frame: Both Roll1 and Roll2 required
   - 10th Frame: Roll3 required only if strike or spare

## 🗃️ Database Schema

### Games Table
- `Id` (int, PK)
- `Name` (string)
- `Score` (int)

### Frames Table
- `Id` (int, PK)
- `GameId` (int, FK)
- `FrameIndex` (int, 0-9)
- `Roll1` (int)
- `Roll2` (int, nullable)
- `Roll3` (int, nullable)
- `Score` (int)

### HighScores Table
- `Id` (int, PK)
- `Name` (string, max 50)
- `Score` (int)
- `DateAchieved` (DateTime)

## 🔧 Configuration

### CORS

Configured to allow requests from Angular frontend:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
```

Update `Program.cs` to add additional allowed origins if needed.

### Environment Settings

- **Development**: Uses `appsettings.Development.json`
- **Production**: Uses `appsettings.json`

## 📝 Code Quality Features

### Clean Code Principles
- **Single Responsibility**: Each class has one clear purpose
- **Dependency Injection**: Constructor injection throughout
- **Interface Segregation**: Clear interfaces for services and repositories
- **DRY**: Validation logic extracted to separate service
- **Constants**: Magic numbers replaced with named constants

### Design Patterns
- **Repository Pattern**: Abstracts data access
- **Service Pattern**: Encapsulates business logic
- **Strategy Pattern**: Validation strategies for different frame types

### Best Practices
- ✅ Async/await for all database operations
- ✅ Nullable reference types enabled
- ✅ Input validation with data annotations
- ✅ Proper HTTP status codes
- ✅ XML documentation comments
- ✅ Centralized constants and error messages

## 🧪 Testing

Run tests (when implemented):

```bash
dotnet test
```

## 📦 NuGet Packages

- **Microsoft.EntityFrameworkCore.SqlServer** (8.0.22): SQL Server provider
- **Microsoft.EntityFrameworkCore.Tools** (8.0.22): EF Core CLI tools
- **Microsoft.EntityFrameworkCore.Design** (8.0.22): Design-time support
- **Swashbuckle.AspNetCore** (6.6.2): Swagger/OpenAPI

## 🔐 Security Considerations

- Input validation on all endpoints
- SQL injection protection via parameterized queries (EF Core)
- CORS configuration restricts origins
- HTTPS enforcement in production

## 🚀 Deployment

### Publish for Production

```bash
dotnet publish -c Release -o ./publish
```

### Update Connection String

Set production connection string in environment variables or `appsettings.Production.json`.

### Run Migrations on Production

```bash
dotnet ef database update --connection "YourProductionConnectionString"
```

## 🐛 Troubleshooting

### Database Connection Issues
- Verify SQL Server is running
- Check connection string in `appsettings.json`
- Ensure database exists or run migrations

### Migration Issues
```bash
# Remove last migration
dotnet ef migrations remove

# Create new migration
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update
```

## 📖 Additional Resources

- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- [C# Coding Conventions](https://docs.microsoft.com/dotnet/csharp/fundamentals/coding-style)

## 👨‍💻 Development Notes

This API demonstrates professional .NET development practices:

- **Clean Architecture**: Clear separation between layers
- **SOLID Principles**: Single responsibility, dependency inversion
- **Async Programming**: Non-blocking database operations
- **Type Safety**: Nullable reference types, strong typing
- **Maintainability**: Constants, validation service, clear naming
- **Documentation**: XML comments, comprehensive README
- **Error Handling**: Validation at multiple levels

## 🎓 Bowling Score Calculation Example

### Strike Example (Frame 1)
- Roll1: 10 (Strike)
- Next frame Roll1: 7, Roll2: 2
- **Frame 1 Score**: 10 + 7 + 2 = **19**

### Spare Example (Frame 2)
- Roll1: 7, Roll2: 3 (Spare)
- Next frame Roll1: 5
- **Frame 2 Score**: 10 + 5 = **15**

### Double Strike Example
- Frame 1: Strike (10)
- Frame 2: Strike (10)
- Frame 3 Roll1: 7
- **Frame 1 Score**: 10 + 10 + 7 = **27**
- **Frame 2 Score**: 10 + 7 + (next roll)

## ✅ Improvements Implemented

This version includes several clean code improvements:

1. **Constants File**: Centralized magic numbers and error messages
2. **Validation Service**: Extracted validation logic to separate service
3. **Data Annotations**: Input validation on DTOs
4. **Code Organization**: Clear separation of concerns
5. **Type Safety**: Proper use of nullable types
6. **Documentation**: XML comments on public APIs

