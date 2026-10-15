using Microsoft.Extensions.Diagnostics.HealthChecks;
using YeMovies.Application.Database;

namespace YeMoviesApi.Health;

public class DatabaseHealthCheck : IHealthCheck
{
    public const string Name = "Database";
    
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ILogger<DatabaseHealthCheck> _logger;

    public DatabaseHealthCheck(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = new())
    {
        try
        {
            _ = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            return HealthCheckResult.Healthy();
        }
        catch (Exception e)
        {
            const string errorMessage = "Database is unhealthy";
            _logger.LogError(errorMessage, e);
            return HealthCheckResult.Unhealthy(errorMessage, e);
        }
    }
}