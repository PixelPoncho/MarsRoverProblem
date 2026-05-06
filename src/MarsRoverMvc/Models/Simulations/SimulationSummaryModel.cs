using MarsRoverMvc.Models.Rovers;

namespace MarsRoverMvc.Models.Simulations
{
    public class SimulationSummary
    {
        /// Unique simulation identifier
        public string SimulationId { get; set; } = string.Empty;

        /// Maximum X coordinate of the plateau (width)
        public int PlateauMaxX { get; set; }

        /// Maximum Y coordinate of the plateau (height)
        public int PlateauMaxY { get; set; }

        /// Plateau dimensions (e.g., "5 x 5")
        public string PlateauSize => $"{PlateauMaxX} x {PlateauMaxY}" ;

        /// Timestamp of execution
        public DateTime ExecutedAt { get; set; }

        /// Final results for each rover
        public List<RoverResultData> Results { get; set; } = new();

        //Screenshot data for the rover
        public string ScreenshotDataUri { get; set; } = string.Empty;
    }
}