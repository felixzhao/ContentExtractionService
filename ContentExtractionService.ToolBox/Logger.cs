using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using log4net;
using log4net.Config;

namespace ContentExtractionService.ToolBox.Logger
{
    public static class Logger
    {
        private static ILog _log;

        private static void EnsureLogger()
        {
            if (_log != null) return;

            var assembly = Assembly.GetEntryAssembly();
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            var configFile = GetConfigFile();

            // Configure Log4Net
            XmlConfigurator.Configure(logRepository, configFile);
            _log = LogManager.GetLogger(assembly, assembly.ManifestModule.Name.Replace(".dll", "").Replace(".", " "));
        }

        private static FileInfo GetConfigFile()
        {
            FileInfo configFile = null;

            // Search config file
            var configFileNames = new[] { "Config/log4net.config", "log4net.config" };

            foreach (var configFileName in configFileNames)
            {
                configFile = new FileInfo(configFileName);

                if (configFile.Exists) break;
            }

            if (configFile == null || !configFile.Exists) throw new NullReferenceException("Log4net config file not found.");

            return configFile;
        }

        public static void Log(string message)
        {
            EnsureLogger();

            _log.Debug($"Test log: {message}");
        }
    }

   
}