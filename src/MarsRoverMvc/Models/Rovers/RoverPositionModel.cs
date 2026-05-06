namespace MarsRoverMvc.Models.Rovers
{
    /// Represents a rover position at a point in time
    public class RoverPosition
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Direction { get; set; } = string.Empty;
    }
}