using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SyllabusManager.Data;
using SyllabusManager.Data.ProviderContexts;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SyllabusManager.DbMigrator
{
    public class Program
    {
        private static IConfigurationRoot _configuration;
        public static async Task Main()
        {
            LoadConfiguration();

            var inputDbProvider = _configuration.GetValue<string>("InputDatabaseProvider");
            var outputDbProvider = _configuration.GetValue<string>("OutputDatabaseProvider");

            while (true)
            {
                Console.Write($"Do you confirm migrating data from \"{inputDbProvider}\" to \"{outputDbProvider}\"? [y/N]: ");
                var response = Console.ReadLine();
                if (response?.Equals("y", StringComparison.OrdinalIgnoreCase) ?? false)
                {
                    Console.WriteLine("Migrating...");
                    await MigrateData(inputDbProvider, outputDbProvider);
                    break;
                }

                if (!(response?.Equals("n", StringComparison.OrdinalIgnoreCase) ?? false) &&
                    !string.IsNullOrWhiteSpace(response)) continue;
                Console.WriteLine("Migration cancelled");
                break;
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

        private static async Task MigrateData(string inputDbProvider, string outputDbProvider)
        {
            var successCount = 0;
            var inputDbContext = GetDbContext(inputDbProvider, "Input");
            var outputDbContext = GetDbContext(outputDbProvider, "Output");

            try
            {
                // User
                await outputDbContext.Users.AddRangeAsync(inputDbContext.Users);
                await outputDbContext.SaveChangesAsync();
                successCount++;
                await outputDbContext.Roles.AddRangeAsync(inputDbContext.Roles);
                await outputDbContext.SaveChangesAsync();
                successCount++;
                await outputDbContext.UserRoles.AddRangeAsync(inputDbContext.UserRoles);
                await outputDbContext.SaveChangesAsync();
                successCount++;

                // Field of study
                await outputDbContext.FieldsOfStudies.AddRangeAsync(
                    inputDbContext.FieldsOfStudies
                    .Include(f => f.Supervisor)
                    .Include(f => f.Specializations)
                    );
                await outputDbContext.SaveChangesAsync();
                successCount++;

                // Learning outcome
                await outputDbContext.LearningOutcomeDocuments.AddRangeAsync(
                    inputDbContext.LearningOutcomeDocuments
                        .Include(l => l.LearningOutcomes)
                        .ThenInclude(lo => lo.Specialization)
                        .Include(l => l.FieldOfStudy)
                );
                await outputDbContext.SaveChangesAsync();
                successCount++;

                // Subject
                await outputDbContext.Subjects.AddRangeAsync(
                    inputDbContext.Subjects
                        .Include(s => s.FieldOfStudy)
                        .Include(s => s.Specialization)
                        .Include(s => s.Supervisor)
                        .Include(s => s.SubjectsTeachers)
                        .ThenInclude(st => st.Subject)
                        .Include(s => s.SubjectsTeachers)
                        .ThenInclude(st => st.Teacher)
                        .Include(s => s.CardEntries)
                        .ThenInclude(ce => ce.Entries)
                        .Include(s => s.LearningOutcomeEvaluations)
                        .Include(s => s.Literature)
                        .Include(s => s.Lessons)
                        .ThenInclude(l => l.ClassForms)
                );
                await outputDbContext.SaveChangesAsync();
                successCount++;

                // Syllabus
                await outputDbContext.Syllabuses.AddRangeAsync(
                    inputDbContext.Syllabuses
                        .Include(s => s.FieldOfStudy)
                        .Include(s => s.Specialization)
                        .Include(s => s.Description)
                        .Include(s => s.PointLimits)
                        .Include(s => s.SubjectDescriptions)
                        .ThenInclude(sd => sd.Subject)
                );
                await outputDbContext.SaveChangesAsync();
                successCount++;

                Console.WriteLine("Successfully migrated all data!");
            }
            catch (Exception e)
            {
                Console.WriteLine($".Migration failed! Migrated with success {successCount} tables. Exception:");
                Console.WriteLine(e);
            }
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
                case "Postgres":
                    var connectionStringPostgres = _configuration.GetValue<string>($"ConnectionStrings:{type}Database");
                    var optionsBuilderPostgres = new DbContextOptionsBuilder<PostgresSyllabusManagerDbContext>().UseNpgsql(connectionStringPostgres);
                    return new PostgresSyllabusManagerDbContext(optionsBuilderPostgres.Options);
                default:
                    throw new Exception("No valid database provider! Available options: SqlServer, Oracle, Postgres.");
            }
        }
    }
}
