namespace MarsRoverMvc.Models.Api
{
  /// DTO for simulation response from the API
  public class SimulationResponse
  {
    public string SimulationId { get; set; } = string.Empty;

    public List<RoverOutput> Rovers { get; set; } = new();

    public DateTime ExecutedAt { get; set; }
  }
}