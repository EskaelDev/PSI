using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using SyllabusManager.Data;
using SyllabusManager.Data.ProviderContexts;

namespace SyllabusManager.DbMigrator
{
    public class Program
    {
        private static IConfigurationRoot _configuration;
        public static void Main()
        {
            LoadConfiguration();

            var inputDbProvider = _configuration.GetValue<string>("InputDatabaseProvider");
            var outputDbProvider = _configuration.GetValue<string>("OutputDatabaseProvider");

            while (true)
            {
                Console.Write($"Do you confirm migrating data from \"{inputDbProvider}\" to \"{outputDbProvider}\"? [y/N]: ");
                var response = Console.ReadLine();
                if (response.Equals("y", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Migrating...");
                    MigrateData(inputDbProvider, outputDbProvider);
                    Console.WriteLine("Migration finished");
                    break;
                }
                if (response.Equals("n", StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(response))
                {
                    Console.WriteLine("Migration cancelled");
                    break;
                }
            }
            Console.ReadKey();
        }

        private static void LoadConfiguration()
        {
            _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false)
            .Build();
        }

        private static void MigrateData(string inputDbProvider, string outputDbProvider)
        {
            var inputDbContext = GetDbContext(inputDbProvider, "Input");
            var outputDbContext = GetDbContext(outputDbProvider, "Output");

            outputDbContext.Users.AddRange(inputDbContext.Users);

            // todo: Migrate data
            outputDbContext.SaveChanges();
        }

        private static SyllabusManagerDbContext GetDbContext(string provider, string type)
        {
            switch (provider)
            {
                case "SqlServer":
                    var connectionStringSqlServer = _configuration.GetValue<string>($"ConnectionStrings:{type}Database");
                    var optionsBuilderSqlServer = new DbContextOptionsBuilder<SqlServerSyllabusManagerDbContext>().UseSqlServer(connectionStringSqlServer);
                    return new SqlServerSyllabusManagerDbContext(optionsBuilderSqlServer.Options);
                case "Oracle":
                    var connectionStringOracle = _configuration.GetValue<string>($"ConnectionStrings:{type}Database");
                    var optionsBuilderOracle = new DbContextOptionsBuilder<OracleSyllabusManagerDbContext>().UseOracle(connectionStringOracle);
                    return new OracleSyllabusManagerDbContext(optionsBuilderOracle.Options);
                default:
                    throw new Exception("No valid database provider! Available options: SqlServer, Oracle.");
            }
        }
    }
}
