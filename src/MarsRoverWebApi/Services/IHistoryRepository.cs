namespace MarsRoverWebApi.Services
{
    /// Interface for simulation history persistence
    /// Abstracts the storage mechanism (JSON file, database, etc.)
    public interface IHistoryRepository
    {
        /// Saves a simulation result to history
        Task SaveSimulationAsync(object simulation);

        /// Retrieves all historical simulations
        Task<List<object>> GetAllSimulationsAsync();

        /// Saves a screenshot of a simulation
        Task SaveScreenshotAsync(string simulationId, string screenshotBase64);
    }
}
