using Akka.Configuration;
using Akka.Configuration.Hocon;
using Microsoft.Azure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocAkkaCastleWindsor.Actors
{
    public static class AzureRuntimeBootloader
    {
        public static Config CreateConfig()
        {
            var section = (AkkaConfigurationSection)ConfigurationManager.GetSection("akka");
            var originalConfig = section.AkkaConfig;

            string settingConnectionString = GetConnectionString();

            var journalPersistenceString = string.Format(@"akka.persistence.journal.sql-server.connection-string = ""{0}""", settingConnectionString);
            var journalConfig = ConfigurationFactory.ParseString(journalPersistenceString);

            var snapshotPersistenceString = string.Format(@"akka.persistence.snapshot-store.sql-server.connection-string = ""{0}""", settingConnectionString);
            var snapshotConfig = ConfigurationFactory.ParseString(snapshotPersistenceString);

            var finalConfig = journalConfig
                                    .WithFallback(snapshotConfig)
                                    .WithFallback(originalConfig);
            return finalConfig;
        }

        private static string GetConnectionString()
        {
            if (!bool.TryParse(
                CloudConfigurationManager.GetSetting("UseAppConfigConnectionString"),
                out bool useAppConfigConnectionString))
            {
                useAppConfigConnectionString = false;
            }

            string settingConnectionString;

            if (useAppConfigConnectionString)
            {
                settingConnectionString = ConfigurationManager.ConnectionStrings["SQLConnectionStringAKKAPersistence"].ConnectionString;
            }
            else
            {
                settingConnectionString = CloudConfigurationManager.GetSetting("SQLConnectionStringAKKAPersistence");
            }

            return settingConnectionString;
        }
    }
}
