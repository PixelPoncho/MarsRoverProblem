namespace MarsRoverMvc.Models.Api
{
    /// DTO for individual rover input
    public class RoverInputData
    {
        public int StartX { get; set; }
        public int StartY { get; set; }
        public string StartDirection { get; set; } = "N";
        public string Commands { get; set; } = string.Empty;
    }
}