using HappyTravel.VaultClient;
using HappyTravel.Wakayama.Common.Helpers;

namespace HappyTravel.Wakayama.Updater.Infrastracture.Helpers;

public static class VaultClientHelper
{
    public static VaultClient.VaultClient GetVaultClient(IConfiguration configuration)
    { 
        var vaultClient = new VaultClient.VaultClient(new VaultOptions
        {
            BaseUrl = new Uri(EnvironmentVariableHelper.Get("Vault:Endpoint", configuration)),
            Engine = configuration["Vault:Engine"],
            Role = configuration["Vault:Role"]
        });
        
        vaultClient.Login(EnvironmentVariableHelper.Get("Vault:Token", configuration)).GetAwaiter().GetResult();

        return vaultClient;
    }
}