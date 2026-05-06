namespace MarsRoverMvc.Models.Api
{
    public class SimulationRequest
    {
        public int PlateauMaxX { get; set; }
        public int PlateauMaxY { get; set; }

        public List<RoverInputData> Rovers { get; set; } = new();
    }
}