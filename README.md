# Mars Rover Problem - ASP.NET Core MVC Solution

An implementation of the following coding challenge using ASP.NET Core 10 MVC architecture.

## Problem:

A squad of robotic rovers are to be landed by NASA on a plateau on Mars. This plateau, which is
curiously rectangular, must be navigated by the rovers so that their on-board cameras can get a
complete view of the surrounding terrain to send back to Earth. A rover's position and location is
represented by a combination of x and y co-ordinates and a letter representing one of the four cardinal compass points.

The plateau is divided up into a grid to simplify navigation. An example position might be 0, 0, N, which means the rover is in the bottom left corner and facing North. In order to control a rover, NASA sends a simple string of letters. The possible letters are 'L', 'R' and 'M'. 'L' and 'R' makes the rover spin 90 degrees left or right respectively, without moving from its current spot. 'M' means move forward one grid point and maintain the same heading.

Assume that the square directly North from (x, y) is (x, y+1).

---
# Table of Contents

- [Overview](#overview)
- [Project Structure](#project-structure)
- [Screenshots](#screenshots)
- [Running the Application](#running-the-application)
- [Using the Application](#using-the-application)
- [How It Works](#how-it-works)
- [Core Architecture](#core-architecture)
- [Test Cases](#test-cases)
- [Customization](#customization)
- [API Endpoints](#api-endpoints)
- [Troubleshooting](#troubleshooting)
- [Future Enhancements](#future-enhancements)
---

# Overview

This solution is split into two applications:

- **Web API (`MarsRoverWebApi`)**
  - Handles all business logic and simulation processing
  - Provides RESTful endpoints
  - JSON-based history persistence
  - Screenshot storage support
  - Swagger/OpenAPI documentation

- **MVC Application (`MarsRoverMvc`)**
  - Provides user interface
  - Canvas-based visualization
  - Rover path tracking
  - History page with simulation records
  - Communicates with the Web API
  - Screenshot preview and storage

# Project Structure

```
MarsRoverProblem/
├── src/
│   ├── MarsRoverWebApi/              # RESTful Web Service (Port 5001/5002)
│   │   ├── Controllers/
│   │   │   └── RoverController.cs    # API endpoints for simulation
│   │   ├── Models/
│   │   │   ├── Plateau.cs           # Plateau domain model
│   │   │   ├── Rover.cs             # Rover domain model
│   │   │   ├── RoverPosition.cs     # Position and direction tracking
│   │   │   ├── SimulationRequest.cs # API request DTOs
│   │   │   └── SimulationResponse.cs # API response DTOs
│   │   ├── Services/
│   │   │   ├── IRoverSimulationService.cs     # Simulation interface
│   │   │   ├── RoverSimulationService.cs      # Core rover logic
│   │   │   └── IHistoryRepository.cs          # History persistence interface
│   │   ├── Data/
│   │   │   └── JsonHistoryRepository.cs       # JSON-based history storage
│   │   ├── Program.cs                         # Configuration & dependency injection
│   │   ├── appsettings.json
│   │   └── appsettings.Development.json
│   │
│   └── MarsRoverMvc/                 # ASP.NET Core MVC App (Port 5035/5036)
│       ├── Controllers/
│       │   ├── HomeController.cs    # Home page navigation
│       │   ├── SimulationController.cs # Simulation form & results
│       │   └── HistoryController.cs   # History page
│       ├── Views/
│       │   ├── Shared/
│       │   │   └── _Layout.cshtml   # Master layout with sidebar
│       │   ├── Home/
│       │   │   ├── Index.cshtml     # Dashboard
│       │   ├── Simulation/
│       │   │   ├── Index.cshtml     # Simulation form
│       │   ├── History/
│       │   │   └── Index.cshtml     # History table
│       │   └── _ViewStart.cshtml    # View initialization
│       ├── Models/
│       │   ├── SimulationViewModel.cs  # Simulation input/output models
│       │   └── HistoryViewModel.cs     # History display models
│       ├── Services/
│       │   ├── IRoverApiService.cs  # API communication interface
│       │   └── RoverApiService.cs   # HTTP client for Web API
│       ├── Program.cs                # Configuration & DI
│       ├── appsettings.json
│       └── appsettings.Development.json
│
├── MarsRoverProblem.sln             # Solution file
└── README.md                         # This file
```
## Screenshots
<img width="929" height="749" alt="image" src="https://github.com/user-attachments/assets/c0088082-4d4c-431d-a135-e9dd1b5eeb6d" />
<img width="1438" height="854" alt="image" src="https://github.com/user-attachments/assets/060054e4-9109-49fb-af32-bb1214b14768" />
<img width="1403" height="808" alt="image" src="https://github.com/user-attachments/assets/89ce611a-9816-46bd-ad69-b2dfd6a16756" />

# Running the Application

## Prerequisites

- .NET 10 SDK
- Visual Studio 2022 or VS Code

## Setup

```bash
git clone https://github.com/PixelPoncho/MarsRoverProblem.git
cd MarsRoverProblem
dotnet build
```

## Run Services

### 1. Start Web API

```bash
cd src/MarsRoverWebApi
dotnet run
```

Runs on: https://localhost:5001 or http://localhost:5002

### 2. Start MVC App

```bash
cd src/MarsRoverMvc
dotnet run
```

Runs on: https://localhost:5036 or http://localhost:5035

### 3. Open in Browser:

```
https://localhost:5036
```

# Using the Application

## Simulation Page

1. Set plateau dimensions (Width × Height)
2. Add rovers:
   - Start position (X, Y)
   - Direction (N, E, S, W)
   - Commands (L, R, M)
3. Click Run Simulation
4. View results and visualization

## History Page

- View all past simulations
- See rover results and timestamps
- View stored screenshots when available

# How It Works

## Simulation Flow

1. **User Input** (MVC App)
   - Sets plateau dimensions (e.g., 5x5)
   - Adds rovers with starting positions and commands
   - Example: Rover at (1, 2, N) with commands "LMLMLMLMM"

2. **HTTP Request to Web API**

   ```
   POST /api/rover/simulate
   {
     "plateauMaxX": 5,
     "plateauMaxY": 5,
     "rovers": [
       {
         "startX": 1,
         "startY": 2,
         "startDirection": "N",
         "commands": "LMLMLMLMM"
       }
     ]
   }
   ```

3. **Simulation Processing** (Web API)
   - Creates Plateau object with bounds
   - For each rover sequentially:
     - Parse each command (L, R, or M)
     - L: Rotate counter-clockwise (N→W→S→E→N)
     - R: Rotate clockwise (N→E→S→W→N)
     - M: Move forward if in bounds, record position
   - Save simulation to history

4. **Response with Results** (Web API)

   ```json
   {
     "simulationId": "uuid",
     "rovers": [
       {
         "roverId": 1,
         "finalX": 1,
         "finalY": 3,
         "finalDirection": "N",
         "path": ["1 2 N", "1 3 N", ...],
         "commands": "LMLMLMLMM"
       }
     ],
     "executedAt": "2026-04-30T20:56:13Z"
   }
   ```

5. **Visualization** (MVC App)
   - Renders plateau as grid on HTML Canvas
   - Draws rover paths
   - Shows start positions (colored numbered squared)
   - Shows final positions (colored numbered squared)
   - Screenshot saved to Web API

6. **History Storage** (Web API)
   - All simulations saved to `Data/History/simulations.json`
   - Screenshots saved to `Data/Screenshots/`
   - Retrieved via GET /api/rover/history

# Core Architecture

## Key Components

### Web API (MarsRoverWebApi)

**Purpose:** RESTful service containing all business logic

**Key Components:**

- **RoverSimulationService**: Core algorithm that processes rover commands
  - Handles rotation logic (L/R turns)
  - Handles movement logic (M command)
  - Tracks position history for visualization
  - Validates bounds checking

- **JsonHistoryRepository**: Persistence layer
  - Saves all simulations to `Data/History/simulations.json`
  - Stores screenshots in `Data/Screenshots/`
  - No database required (file-based for simplicity)

### MVC Application (MarsRoverMvc)

**Purpose:** User-friendly interface for simulation

**Key Components:**

- **SimulationController**: Handles simulation workflow
  - Form page for input (plateau + rovers)
  - Results page with visualization
  - Calls Web API for processing

- **HistoryController**: Displays past simulations
  - Retrieves history from Web API
  - Shows results in sorted table

- **RoverApiService**: HTTP client
  - Communicates with Web API
  - Serializes/deserializes JSON

- **Plateau Visualization**: Canvas-based drawing
  - Draws grid based on plateau size
  - Shows rover paths
  - Marks start position (coloured number squares)
  - Marks final positions (coloured number squares)
  - Captures screenshot functionality

## Test Cases

### Test Case 1: Basic Movement

```
Plateau: 5 5
Rover 1: 1 2 N → LMLMLMLMM → Expected: 1 3 N
```

### Test Case 2: Two Rovers

```
Plateau: 5 5
Rover 1: 1 2 N → LMLMLMLMM → Expected: 1 3 N
Rover 2: 3 3 E → MMRMMRMRRM → Expected: 5 1 E
```

### Test Case 3: Boundary Check

```
Plateau: 2 2
Rover 1: 0 0 N → MMM → Expected: 0 2 N (stops at boundary)
```

# Customization

## Change Ports

Edit `appsettings.json` in each project.

## Modify Plateau Limits

Update input max value in:

```
Simulation/Index.cshtml
```

# API Endpoints

## Simulation

- **POST** `/api/rover/simulate`
  - Input: Plateau dimensions and rovers
  - Output: Final positions and paths
  - Also saves to history automatically

## History

- **GET** `/api/rover/history`
  - Returns all past simulations with results
  - Sorted by date

## Screenshots

- **POST** `/api/rover/save-screenshot`
  - Parameters: `simulationId` (query), `imageBase64` (body)
  - Stores screenshot file on disk

# API Docs

Available in development mode: https://localhost:5002/swagger/index.html

# Troubleshooting

### API not responding

- Check both services are running on correct ports
- Verify firewall settings
- Check CORS policy in API Program.cs

### Simulation not saving

- Ensure `Data/` directory exists and is writable
- Check file permissions
- Verify JSON serialization

### Visualization not showing

- Clear browser cache
- Check browser console for JavaScript errors
- Verify Canvas API support

# Future Enhancements

1. **Database Integration**
   - Replace JsonHistoryRepository with EF Core (DB integration)
   - Add search and filtering
   - Add history entry deletion

2. **Advanced Visualization**
   - Real-time animation of rover movement
   - Collision detection for rover simulation

3. **Collaboration Features**
   - Share simulations with URLs
   - Comments on simulations

4. **Unit Tests**
   - Test RoverSimulationService logic
   - Test boundary conditions
   - Test command parsing

5. **Performance**
   - Caching for frequently accessed simulations
   - Pagination for history
