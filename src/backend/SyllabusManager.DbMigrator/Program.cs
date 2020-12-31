using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SyllabusManager.Data;
using SyllabusManager.Data.ProviderContexts;
using System;
using System.IO;

namespace SyllabusManager.DbMigrator
{
    public class Program
    {
        private static IConfigurationRoot _configuration;
        public static void Main()
        {
            LoadConfiguration();

            string inputDbProvider = _configuration.GetValue<string>("InputDatabaseProvider");
            string outputDbProvider = _configuration.GetValue<string>("OutputDatabaseProvider");

            while (true)
            {
                Console.Write($"Do you confirm migrating data from \"{inputDbProvider}\" to \"{outputDbProvider}\"? [y/N]: ");
                string response = Console.ReadLine();
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
            SyllabusManagerDbContext inputDbContext = GetDbContext(inputDbProvider, "Input");
            SyllabusManagerDbContext outputDbContext = GetDbContext(outputDbProvider, "Output");

            outputDbContext.Users.AddRange(inputDbContext.Users);

            // todo: Migrate data
            outputDbContext.SaveChanges();
        }

        private static SyllabusManagerDbContext GetDbContext(string provider, string type)
        {
            switch (provider)
            {
                case "SqlServer":
                    string connectionStringSqlServer = _configuration.GetValue<string>($"ConnectionStrings:{type}Database");
                    DbContextOptionsBuilder<SqlServerSyllabusManagerDbContext> optionsBuilderSqlServer = new DbContextOptionsBuilder<SqlServerSyllabusManagerDbContext>().UseSqlServer(connectionStringSqlServer);
                    return new SqlServerSyllabusManagerDbContext(optionsBuilderSqlServer.Options);
                case "Oracle":
                    string connectionStringOracle = _configuration.GetValue<string>($"ConnectionStrings:{type}Database");
                    DbContextOptionsBuilder<OracleSyllabusManagerDbContext> optionsBuilderOracle = new DbContextOptionsBuilder<OracleSyllabusManagerDbContext>().UseOracle(connectionStringOracle);
                    return new OracleSyllabusManagerDbContext(optionsBuilderOracle.Options);
                case "Postgres":
                    string connectionStringPostgres = _configuration.GetValue<string>($"ConnectionStrings:{type}Database");
                    DbContextOptionsBuilder<PostgresSyllabusManagerDbContext> optionsBuilderPostgres = new DbContextOptionsBuilder<PostgresSyllabusManagerDbContext>().UseNpgsql(connectionStringPostgres);
                    return new PostgresSyllabusManagerDbContext(optionsBuilderPostgres.Options);
                default:
                    throw new Exception("No valid database provider! Available options: SqlServer, Oracle, Postgres.");
            }
        }
    }
}
