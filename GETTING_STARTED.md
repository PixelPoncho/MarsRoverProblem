# Getting Started - Mars Rover Simulator

This guide will help you set up and run the Mars Rover Simulator application.

## Quick Start

### 1. Clone the Repository
```bash
git clone https://github.com/PixelPoncho/MarsRoverProblem.git
cd MarsRoverProblem
```

### 2. Build the Solution
```bash
dotnet build
```

### 3. Run Both Services

**Terminal 1 - Start Web API:**
```bash
cd src/MarsRoverWebApi
dotnet run
```
You should see:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5001
```

**Terminal 2 - Start MVC App:**
```bash
cd src/MarsRoverMvc
dotnet run
```
You should see:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5000
```

### 4. Open in Browser
Navigate to: `https://localhost:5000`

## Using the Application

### Home Page
- View overview of the application
- Access main features from the sidebar

### New Simulation
1. Set plateau dimensions (Width × Height)
2. Add rovers:
   - Start X, Start Y coordinates
   - Starting direction (N, E, S, W)
   - Command sequence (L, R, M characters)
3. Click "Run Simulation"
4. View results and plateau visualization
5. Capture screenshot if desired

### Viewing History
- Click "History" to see all past simulations
- View plateau dimensions, rover count, and results
- See execution timestamp

### About Page
- Learn about the architecture
- See technologies used
- Understand the feature set

## Example: Test Case from Requirement

### Input
```
Plateau: 5 × 5

Rover 1:
  Position: 1 2 N
  Commands: LMLMLMLMM

Rover 2:
  Position: 3 3 E
  Commands: MMRMMRMRRM
```

### Expected Output
```
Rover 1: 1 3 N
Rover 2: 5 1 E
```

### Steps
1. Go to New Simulation
2. Set Plateau Width: 5, Height: 5
3. Rover 1:
   - Start X: 1, Start Y: 2, Direction: N
   - Commands: LMLMLMLMM
4. Click "+ Add Rover"
5. Rover 2:
   - Start X: 3, Start Y: 3, Direction: E
   - Commands: MMRMMRMRRM
6. Click "Run Simulation"
7. View results - should match expected output
8. See visualization with paths on plateau

## Project Structure

```
MarsRoverProblem/
├── src/
│   ├── MarsRoverWebApi/          ← Web Service (Port 5001)
│   │   ├── Controllers/           ← API endpoints
│   │   ├── Models/                ← Domain models
│   │   ├── Services/              ← Business logic
│   │   ├── Data/                  ← Persistence
│   │   └── Program.cs             ← Configuration
│   │
│   └── MarsRoverMvc/              ← MVC App (Port 5000)
│       ├── Controllers/           ← Page controllers
│       ├── Views/                 ← HTML/Razor templates
│       ├── Models/                ← View models
│       ├── Services/              ← API client
│       └── Program.cs             ← Configuration
│
└── README.md
```

## Understanding the Code

### Core Simulation Logic
See: `src/MarsRoverWebApi/Services/RoverSimulationService.cs`

Key methods:
- `Simulate()` - Main orchestration
- `ExecuteRoverCommands()` - Process L/R/M commands
- `RotateRover()` - Change direction
- `MoveRover()` - Update position with bounds checking

### API Communication
See: `src/MarsRoverMvc/Services/RoverApiService.cs`

Handles:
- HTTP POST to `/api/rover/simulate`
- HTTP GET to `/api/rover/history`
- HTTP POST to `/api/rover/save-screenshot`

### Data Persistence
See: `src/MarsRoverWebApi/Data/JsonHistoryRepository.cs`

Stores:
- `Data/History/simulations.json` - All simulation records
- `Data/Screenshots/` - Base64-encoded PNG screenshots

## Customization

### Change Ports
Edit `appsettings.json` in each project:
```json
"Kestrel": {
  "Endpoints": {
    "Http": { "Url": "http://localhost:5000" },
    "Https": { "Url": "https://localhost:5001" }
  }
}
```

### Modify Plateau Size Limits
In `Simulation/Index.cshtml`:
```html
<input type="number" ... max="50" /> <!-- Change this -->
```

### Change Theme Colors
In `Shared/_Layout.cshtml`, look for CSS variables:
```css
background: linear-gradient(135deg, #4CAF50, #45a049);
```

## Troubleshooting

### "Connection refused" error
- Ensure both services are running
- Check ports 5000 and 5001 are not in use
- Try: `netstat -tuln | grep -E '5000|5001'`

### "CORS error" in browser console
- Verify API service is running
- Check CORS policy in Web API `Program.cs`
- Clear browser cache

### Simulation data not saving
- Check `Data/History/` directory exists
- Verify write permissions
- Check Windows Defender/antivirus isn't blocking

### Canvas visualization not showing
- Ensure JavaScript is enabled
- Check browser console for errors (F12)
- Try different browser (Chrome recommended)

## Next Steps

1. **Run a test simulation** - Use the example above
2. **Explore the code** - Read the inline comments
3. **Check history** - View saved simulations
4. **Capture screenshots** - Test the screenshot feature
5. **Review architecture** - See `ARCHITECTURE.md`

## Need Help?

Refer to:
- Code comments in each file
- ARCHITECTURE.md for design details
- Inline documentation in classes and methods

## Support

API Documentation available at:
```
https://localhost:5001/swagger/index.html
```
(When running in Development mode)
