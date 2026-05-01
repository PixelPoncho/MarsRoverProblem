# Mars Rover Problem - ASP.NET Core MVC Solution

A complete implementation of the NASA Mars Rover coding challenge using ASP.NET Core 10 MVC architecture.

## Project Structure

```
MarsRoverProblem/
├── src/
│   ├── MarsRoverWebApi/          # RESTful Web Service
│   │   ├── Controllers/
│   │   ├── Models/
│   │   ├── Services/
│   │   ├── Data/
│   │   └── Program.cs
│   │
│   └── MarsRoverMvc/             # ASP.NET Core MVC Application
│       ├── Controllers/
│       ├── Views/
│       ├── Models/
│       ├── Services/
│       └── Program.cs
└── README.md
```

## Features

### Web Service
- RESTful API endpoints for rover simulation
- Input/output history persistence (JSON-based)
- Complete simulation state management

### MVC Application
- Intuitive UI for plateau and rover configuration
- Real-time plateau visualization
- Path tracking and final position display
- History page with all past simulations
- Screenshot capture and storage

## Running the Application

### Prerequisites
- .NET 10 SDK
- Visual Studio 2022 or VS Code

### Setup

1. Build the solution:
   ```bash
   dotnet build
   ```

2. Run Web API (port 502):
   ```bash
   cd src/MarsRoverWebApi
   dotnet run
   ```

3. Run MVC App (port 5036):
   ```bash
   cd src/MarsRoverMvc
   dotnet run
   ```

4. Open browser to `http://localhost:5036`

## API Endpoints

### POST /api/rover/simulate
Run a simulation with given plateau and rovers.

### GET /api/history
Retrieve all historical simulations.

### POST /api/history/save-screenshot
Save a screenshot of the plateau visualization.

## Architecture

- **Separation of Concerns**: Web API handles business logic, MVC handles presentation
- **Repository Pattern**: Data access abstraction
- **SOLID Principles**: Clean, maintainable code
- **RESTful Design**: Standard HTTP conventions

## Testing

Test Input:
```
Plateau: 5 5
Rover 1: Position 1 2 N, Commands: LMLMLMLMM
Rover 2: Position 3 3 E, Commands: MMRMMRMRRM
```

Expected Output:
```
Rover 1: 1 3 N
Rover 2: 5 1 E
```
