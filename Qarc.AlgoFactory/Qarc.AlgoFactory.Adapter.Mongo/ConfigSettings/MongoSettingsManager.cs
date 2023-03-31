using Microsoft.Extensions.Configuration;

namespace Qarc.AlgoFactory.Adapter.Mongo.ConfigSettings
{
    public class MongoSettingsManager : IMongoSettingsManager
    {
        private readonly IConfiguration _configuration;
        public MongoSettingsManager(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public string Host { get { return this._configuration["Mongo:MongoSettings:Host"]; } }
        public string Port { get { return this._configuration["Mongo:MongoSettings:Port"]; } }
        public string User { get { return this._configuration["Mongo:MongoSettings:User"]; } }
        public string Pass { get { return this._configuration["Mongo:MongoSettings:Pass"]; } }
        public string ServiceDbName { get { return this._configuration["Mongo:ServiceDbSettings:ServiceDbName"]; } }
        public string ConnectionString { get { return $"mongodb://{User}:{Pass}@{Host}:{Port}/?authSource=admin"; } }

        public IConfigurationSection GetConfigurationSection(string key)
        {
            return this._configuration.GetSection(key);
        }
    }
}
