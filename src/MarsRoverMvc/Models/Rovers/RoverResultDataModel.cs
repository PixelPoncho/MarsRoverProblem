namespace MarsRoverMvc.Models.Rovers
{
    /// Data for a single rover result
    public class RoverResultData
    {
        public int RoverId { get; set; }

        public int FinalX { get; set; }
        public int FinalY { get; set; }

        public string FinalDirection { get; set; } = string.Empty;

        public List<RoverPosition> Path { get; set; } = new();

        public string Commands { get; set; } = string.Empty;
    }
}