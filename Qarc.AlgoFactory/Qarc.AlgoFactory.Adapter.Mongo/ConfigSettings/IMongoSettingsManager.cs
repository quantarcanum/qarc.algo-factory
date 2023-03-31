using Microsoft.Extensions.Configuration;

namespace Qarc.AlgoFactory.Adapter.Mongo.ConfigSettings
{
    public interface IMongoSettingsManager
    {
        string Host { get; }
        string Port { get; }
        string User { get; }
        string Pass { get; }
        string ServiceDbName { get; }
        string ConnectionString { get; }
        IConfigurationSection GetConfigurationSection(string key);
    }
}
