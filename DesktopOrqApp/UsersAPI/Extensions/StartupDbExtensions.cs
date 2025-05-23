using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using DAL.Entities;
using DAL;

namespace UsersAPI.Extensions
{
    public static class StartupDbExtensions
    {
        public static void CreateDbIfNotExists(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var studentContext = services.GetRequiredService<UserDBContext>();

            var logger = services.GetRequiredService<ILogger<Program>>();

            //DBInitializerSeedData.InitializeDatabase(studentContext);

            try
            {
                var databasecrate = studentContext.Database.GetService<IDatabaseCreator>()
                    as RelationalDatabaseCreator;
                if (databasecrate != null)
                {
                    if (!databasecrate.CanConnect())
                    {

                        databasecrate.Create();
                        logger.LogInformation("enter databsecrate Create");
                    }
                    if (!databasecrate.HasTables())
                    {
                        databasecrate.CreateTables();
                        logger.LogInformation("enter databsecrate CreateTables");
                    }

                    DBInitializerSeedData.InitializeDatabase(studentContext);
                    logger.LogInformation("enter databsecrate InitializeDatabase");
                }

            }
            catch (Exception ex)
            {

                logger.LogError($"migration issue {ex.Message}");

            }
        }
    }
}
