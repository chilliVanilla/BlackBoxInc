using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace BlackBoxInc.Data.Factories
{
    public class AuthDbContextFactory : IDesignTimeDbContextFactory<AuthDbContext>
    {

        public AuthDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AuthDbContext>();

            optionsBuilder.UseSqlServer(
                configuration.GetConnectionString("AuthDb"));

            return new AuthDbContext(optionsBuilder.Options);
        }
    }
}
