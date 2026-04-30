namespace MarsRoverWebApi.Models
{
    /// Represents a Mars rover with its current state and movement history
    /// Each rover has a unique ID for tracking purposes
    public class Rover
    {
        /// Unique identifier for this rover
        public int Id { get; set; }

        /// Current position and heading of the rover
        public RoverPosition CurrentPosition { get; set; }

        /// The command string to be executed (L, R, M)
        /// L = Rotate left 90 degrees
        /// R = Rotate right 90 degrees
        /// M = Move forward one grid point
        public string Commands { get; set; }

        /// History of all positions the rover has visited during execution
        /// Stored as string representations like "1 2 N"
        /// This is used for visualization of the rover's path
        public List<string> PositionHistory { get; set; }

        public Rover(int id, RoverPosition startPosition, string commands)
        {
            Id = id;
            CurrentPosition = startPosition;
            Commands = commands;
            // Initialize history with the starting position
            PositionHistory = new List<string> { startPosition.ToString() };
        }
    }
}
