using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DotNetEnv;

namespace Contractor_Backend.Persistence.DbContext
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            Env.Load(); // Load .env for CLI context

            var connectionString = $"Server={Environment.GetEnvironmentVariable("DB_HOST")};" +
                                   $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
                                   $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
                                   $"User={Environment.GetEnvironmentVariable("DB_USER")};" +
                                   $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};";

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
