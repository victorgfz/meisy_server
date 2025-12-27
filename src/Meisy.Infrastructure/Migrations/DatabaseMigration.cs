using Meisy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Meisy.Infrastructure.Migrations
{
    public static class DatabaseMigration
    {
        public async static Task MigrateDatabase(IServiceProvider serviceProvider)
        {

            var dbContext = serviceProvider.GetRequiredService<MeisyDbContext>();
            await dbContext.Database.MigrateAsync();
        }
    }
}
