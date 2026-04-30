namespace MarsRoverWebApi.Models
{
    /// <summary>
    /// Represents the Mars plateau dimensions
    /// The plateau is rectangular with coordinates starting at (0, 0) in the bottom-left
    /// </summary>
    public class Plateau
    {
        /// <summary>
        /// Maximum X coordinate (width)
        /// Valid X coordinates range from 0 to MaxX
        /// </summary>
        public int MaxX { get; set; }

        /// <summary>
        /// Maximum Y coordinate (height)
        /// Valid Y coordinates range from 0 to MaxY
        /// </summary>
        public int MaxY { get; set; }

        public Plateau(int maxX, int maxY)
        {
            if (maxX < 0 || maxY < 0)
                throw new ArgumentException("Plateau dimensions must be non-negative");

            MaxX = maxX;
            MaxY = maxY;
        }

        /// <summary>
        /// Checks if a given position is within the plateau boundaries
        /// </summary>
        public bool IsWithinBounds(int x, int y)
        {
            return x >= 0 && x <= MaxX && y >= 0 && y <= MaxY;
        }
    }
}
