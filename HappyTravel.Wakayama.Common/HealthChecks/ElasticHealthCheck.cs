using Elasticsearch.Net;
using HappyTravel.Wakayama.Common.ElasticClients;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Nest;

namespace HappyTravel.Wakayama.Common.HealthChecks;

public class ElasticHealthCheck : IHealthCheck
{
    public ElasticHealthCheck(ElasticGeoServiceClient client)
    {
        _client = client;
    }
    
    
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var response = await _client.Client.Cluster.HealthAsync(new ClusterHealthRequest(), cancellationToken);
            
        return !response.IsValid 
            ? HealthCheckResult.Unhealthy(response.ToString()) 
            : GetHealthStatus(response);
    }
        
        
    private HealthCheckResult GetHealthStatus(ClusterHealthResponse clusterHealth)
    {
        return clusterHealth.Status switch
        {
            Health.Red => HealthCheckResult.Unhealthy(clusterHealth.ToString()),
            Health.Yellow => HealthCheckResult.Degraded(clusterHealth.ToString()),
            Health.Green => HealthCheckResult.Healthy("Healthy"),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    

    private readonly ElasticGeoServiceClient _client;
}