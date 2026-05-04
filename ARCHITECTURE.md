# Mars Rover Problem - Complete ASP.NET Core Solution

## Project Structure

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
│       │   │   └── Result.cshtml    # Results with plateau visualization
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

## Architecture Overview

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
  - Shows results in sortable table

- **RoverApiService**: HTTP client
  - Communicates with Web API
  - Serializes/deserializes JSON

- **Plateau Visualization**: Canvas-based drawing
  - Draws grid based on plateau size
  - Shows rover paths
  - Marks start position (green S)
  - Marks final positions (numbered circles)
  - Captures screenshot functionality

## How It Works

### Simulation Flow

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
   - Shows start positions (green circles with 'S')
   - Shows final positions (colored numbered circles)
   - User can capture screenshot
   - Screenshot saved to Web API

6. **History Storage** (Web API)
   - All simulations saved to `Data/History/simulations.json`
   - Screenshots saved to `Data/Screenshots/`
   - Retrieved via GET /api/rover/history

## API Endpoints

### Simulation
- **POST** `/api/rover/simulate`
  - Input: Plateau dimensions and rovers
  - Output: Final positions and paths
  - Also saves to history automatically

### History
- **GET** `/api/rover/history`
  - Returns all past simulations with results
  - Sorted by date

### Screenshots
- **POST** `/api/rover/save-screenshot`
  - Parameters: `simulationId` (query), `imageBase64` (body)
  - Stores screenshot file on disk

## Key Design Decisions

### 1. Separation of Concerns
- **Web API** handles all business logic
- **MVC App** handles presentation only
- API can be consumed by other clients (web, mobile, etc.)

### 2. Direction Enum
```csharp
enum Direction { N, E, S, W }
```
- Type-safe alternative to string
- Easy rotation logic with switch statements

### 3. Position Tracking
```csharp
public List<string> PositionHistory { get; set; }
```
- Records every position after each move
- Used for path visualization
- Useful for debugging

### 4. JSON Persistence
```
Data/
├── History/
│   └── simulations.json      # All simulation records
└── Screenshots/
    ├── {simulationId}.png.base64
    └── ...
```
- No database setup required
- Easy to inspect history files
- Scalable to database later (just change IHistoryRepository implementation)

### 5. Dependency Injection
- Services registered in `Program.cs`
- Constructor injection for testability
- Interfaces for loose coupling

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

## Code Comments

Every file includes:
- **File header**: Purpose and overview
- **Class documentation**: What the class does and why
- **Method documentation**: Parameters, return values, logic
- **Inline comments**: Complex algorithm explanations
- **Why comments**: Design decisions and trade-offs

## Features Implemented

✅ **Web Service**
- RESTful API with proper HTTP verbs
- Swagger/OpenAPI documentation
- Input validation
- Error handling

✅ **MVC Application**
- Intuitive form for plateau and rover configuration
- Dynamic rover addition/removal
- Real-time form validation

✅ **Plateau Visualization**
- Canvas-based grid drawing
- Rover path visualization
- Start position marker (green S)
- Final position markers (numbered circles)
- Color-coded rovers

✅ **History**
- All simulations persisted
- Browsable history page
- Screenshot storage capability
- Detailed results table

✅ **Code Quality**
- Comprehensive comments
- SOLID principles
- Clean architecture
- Testable services
- Type-safe enums

## Future Enhancements

1. **Database Integration**
   - Replace JsonHistoryRepository with EF Core
   - Add search and filtering

2. **Advanced Visualization**
   - Real-time animation of rover movement

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

## Troubleshooting

### API not responding
- Check both services are running on correct ports
- Verify firewall settings
- Check CORS policy in API Program.cs

### Simulation not saving
- Ensure Data directory exists and is writable
- Check file permissions
- Verify JSON serialization

### Visualization not showing
- Clear browser cache
- Check browser console for JavaScript errors
- Verify Canvas API support

## License

This is a solution to the NASA Mars Rover coding challenge.

## Contact

For questions about the implementation, refer to the inline code comments.
