namespace MarsRoverWebApi.Models
{
    /// <summary>
    /// Represents a Mars rover with its current state and movement history
    /// Each rover has a unique ID for tracking purposes
    /// </summary>
    public class Rover
    {
        /// <summary>
        /// Unique identifier for this rover
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Current position and heading of the rover
        /// </summary>
        public RoverPosition CurrentPosition { get; set; }

        /// <summary>
        /// The command string to be executed (L, R, M)
        /// L = Rotate left 90 degrees
        /// R = Rotate right 90 degrees
        /// M = Move forward one grid point
        /// </summary>
        public string Commands { get; set; }

        /// <summary>
        /// History of all positions the rover has visited during execution
        /// Stored as string representations like "1 2 N"
        /// This is used for visualization of the rover's path
        /// </summary>
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
