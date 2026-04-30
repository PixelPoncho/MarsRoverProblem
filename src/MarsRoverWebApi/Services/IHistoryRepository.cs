namespace MarsRoverWebApi.Services
{
    /// <summary>
    /// Interface for simulation history persistence
    /// Abstracts the storage mechanism (JSON file, database, etc.)
    /// </summary>
    public interface IHistoryRepository
    {
        /// <summary>
        /// Saves a simulation result to history
        /// </summary>
        Task SaveSimulationAsync(object simulation);

        /// <summary>
        /// Retrieves all historical simulations
        /// </summary>
        Task<List<object>> GetAllSimulationsAsync();

        /// <summary>
        /// Saves a screenshot of a simulation
        /// </summary>
        Task SaveScreenshotAsync(string simulationId, string screenshotBase64);
    }
}
