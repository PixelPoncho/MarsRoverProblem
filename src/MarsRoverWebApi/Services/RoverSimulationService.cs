using MarsRoverWebApi.Models;

namespace MarsRoverWebApi.Services
{
    /// Service that implements the rover simulation logic
    /// This is the core business logic for the Mars Rover problem
    public class RoverSimulationService : IRoverSimulationService
    {
        public SimulationResponse Simulate(SimulationRequest request)
        {
            // Create a response object to hold all results
            var response = new SimulationResponse();

            // Create the plateau based on input dimensions
            var plateau = new Plateau(request.PlateauMaxX, request.PlateauMaxY);

            // Process each rover sequentially
            // "Each rover will be finished sequentially, which means that the second rover 
            // won't start to move until the first one has finished moving."
            int roverId = 1;
            foreach (var roverInput in request.Rovers)
            {
                // Parse the starting direction string ("N", "E", "S", "W") to enum
                if (!Enum.TryParse<Direction>(roverInput.StartDirection, out var direction))
                {
                    direction = Direction.N; // Default to North if invalid
                }

                // Create a rover with initial position
                var rover = new Rover(
                    roverId,
                    new RoverPosition(roverInput.StartX, roverInput.StartY, direction),
                    roverInput.Commands
                );

                // Execute all commands for this rover
                ExecuteRoverCommands(rover, plateau);

                // Add the rover's results to the response
                response.Rovers.Add(new RoverOutputData
                {
                    RoverId = rover.Id,
                    FinalX = rover.CurrentPosition.X,
                    FinalY = rover.CurrentPosition.Y,
                    FinalDirection = rover.CurrentPosition.Direction.ToString(),
                    Path = rover.PositionHistory,
                    Commands = rover.Commands
                });

                roverId++;
            }

            return response;
        }

        /// Executes all commands for a single rover
        /// Commands can be: L (left turn), R (right turn), M (move forward)
        private void ExecuteRoverCommands(Rover rover, Plateau plateau)
        {
            // Process each character in the command string
            foreach (char command in rover.Commands)
            {
                switch (command)
                {
                    case 'L':
                        // Rotate left 90 degrees (counter-clockwise)
                        RotateRover(rover, turnRight: false);
                        break;
                    case 'R':
                        // Rotate right 90 degrees (clockwise)
                        RotateRover(rover, turnRight: true);
                        break;
                    case 'M':
                        // Move forward one grid point in the direction the rover is facing
                        MoveRover(rover, plateau);
                        break;
                    // Any other character is ignored (e.g., whitespace)
                }
            }
        }

        /// Rotates the rover 90 degrees left or right
        /// This changes the direction but not the position
        private void RotateRover(Rover rover, bool turnRight)
        {
            // Map of direction rotations
            // Left rotation (counter-clockwise):  N->W->S->E->N
            // Right rotation (clockwise):         N->E->S->W->N
            
            rover.CurrentPosition.Direction = turnRight
                ? RotateClockwise(rover.CurrentPosition.Direction)
                : RotateCounterClockwise(rover.CurrentPosition.Direction);
        }

        /// Rotates direction 90 degrees clockwise (right)
        /// N (north) -> E (east) -> S (south) -> W (west) -> N
        private Direction RotateClockwise(Direction current)
        {
            return current switch
            {
                Direction.N => Direction.E,
                Direction.E => Direction.S,
                Direction.S => Direction.W,
                Direction.W => Direction.N,
                _ => Direction.N
            };
        }

        /// Rotates direction 90 degrees counter-clockwise (left)
        /// N (north) -> W (west) -> S (south) -> E (east) -> N
        private Direction RotateCounterClockwise(Direction current)
        {
            return current switch
            {
                Direction.N => Direction.W,
                Direction.W => Direction.S,
                Direction.S => Direction.E,
                Direction.E => Direction.N,
                _ => Direction.N
            };
        }

        /// Moves the rover forward one grid point in its current direction
        /// Only moves if the new position is within plateau bounds
        private void MoveRover(Rover rover, Plateau plateau)
        {
            // Calculate the new position based on current direction
            // "Assume that the square directly North from (x, y) is (x, y+1)"
            var newPosition = rover.CurrentPosition.Clone();

            switch (rover.CurrentPosition.Direction)
            {
                case Direction.N:
                    // Moving North increases Y
                    newPosition.Y++;
                    break;
                case Direction.E:
                    // Moving East increases X
                    newPosition.X++;
                    break;
                case Direction.S:
                    // Moving South decreases Y
                    newPosition.Y--;
                    break;
                case Direction.W:
                    // Moving West decreases X
                    newPosition.X--;
                    break;
            }

            // Only update position if it's within plateau bounds
            if (plateau.IsWithinBounds(newPosition.X, newPosition.Y))
            {
                rover.CurrentPosition = newPosition;
                // Record this position in the history for visualization
                rover.PositionHistory.Add(rover.CurrentPosition.ToString());
            }
            // If out of bounds, the rover doesn't move (ignores the command)
        }
    }
}
