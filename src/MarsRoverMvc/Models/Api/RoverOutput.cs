namespace MarsRoverMvc.Models.Api
{
    /// DTO for individual rover output
    public class RoverOutput
    {
        public int RoverId { get; set; }

        public int FinalX { get; set; }
        public int FinalY { get; set; }

        public string FinalDirection { get; set; } = string.Empty;

        public List<string> Path { get; set; } = new();

        public string Commands { get; set; } = string.Empty;
    }
}