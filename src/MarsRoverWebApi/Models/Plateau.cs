namespace MarsRoverWebApi.Models
{
    /// Represents the Mars plateau dimensions
    /// The plateau is rectangular with coordinates starting at (0, 0) in the bottom-left
    public class Plateau
    {
        /// Maximum X coordinate (width)
        /// Valid X coordinates range from 0 to MaxX
        public int MaxX { get; set; }

        /// Maximum Y coordinate (height)
        /// Valid Y coordinates range from 0 to MaxY
        public int MaxY { get; set; }

        public Plateau(int maxX, int maxY)
        {
            if (maxX < 0 || maxY < 0)
                throw new ArgumentException("Plateau dimensions must be non-negative");

            MaxX = maxX;
            MaxY = maxY;
        }

        /// Checks if a given position is within the plateau boundaries
        public bool IsWithinBounds(int x, int y)
        {
            return x >= 0 && x <= MaxX && y >= 0 && y <= MaxY;
        }
    }
}
