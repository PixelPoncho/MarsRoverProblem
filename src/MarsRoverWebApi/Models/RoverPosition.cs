namespace MarsRoverWebApi.Models
{
    /// <summary>
    /// Represents the direction/heading a rover is facing
    /// Uses compass points: N (North), E (East), S (South), W (West)
    /// </summary>
    public enum Direction
    {
        N, // North (positive Y direction)
        E, // East (positive X direction)
        S, // South (negative Y direction)
        W  // West (negative X direction)
    }

    /// <summary>
    /// Represents a rover's current position and heading on the plateau
    /// </summary>
    public class RoverPosition
    {
        /// <summary>
        /// X coordinate on the plateau
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Y coordinate on the plateau
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Direction the rover is facing (N, E, S, or W)
        /// </summary>
        public Direction Direction { get; set; }

        public RoverPosition(int x, int y, Direction direction)
        {
            X = x;
            Y = y;
            Direction = direction;
        }

        /// <summary>
        /// Creates a copy of this position
        /// Useful for tracking position changes during simulation
        /// </summary>
        public RoverPosition Clone()
        {
            return new RoverPosition(X, Y, Direction);
        }

        public override string ToString()
        {
            return $"{X} {Y} {Direction}";
        }
    }
}
