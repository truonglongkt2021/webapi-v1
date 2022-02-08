using Invedia.Data.EF.Interfaces.DbContext;
using Invedia.Data.EF.Services.DbContext;
using Invedia.Data.EF.Utils.ModelBuilderUtils;
using Invedia.DI.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StreamFile.Core.Utils;
using System.Reflection;

namespace StreamFile.Repository.Infrastructure
{
    [ScopedDependency(ServiceType = typeof(IDbContext))]
    public sealed partial class AppDbContext : BaseDbContext
    {
        public static readonly ILoggerFactory LoggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(
            builder =>
            {
                builder
                    .AddFilter((category, level) =>
                        category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
                    .AddConsole();
            });

        public readonly int CommandTimeoutInSecond = 20 * 60;

        public AppDbContext()
        {
            Database.SetCommandTimeout(CommandTimeoutInSecond);
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.SetCommandTimeout(CommandTimeoutInSecond);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = SystemHelper.AppDb;

                connectionString = "Data Source=45.119.82.72;Initial Catalog=stream-file-api-dev;User ID=sa;Password=h9P33fQ5";

                optionsBuilder.UseSqlServer(connectionString, sqlServerOptionsAction =>
                {
                    sqlServerOptionsAction.MigrationsAssembly(
                        typeof(AppDbContext).GetTypeInfo().Assembly.GetName().Name);
                    //sqlServerOptionsAction.MigrationsAssembly($"{nameof(BigClub)}.WebApi");

                    sqlServerOptionsAction.MigrationsHistoryTable("Migration");
                });
            }

            optionsBuilder.UseLoggerFactory(LoggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Convention for Table name
            modelBuilder.RemovePluralizingTableNameConvention();

            modelBuilder.ReplaceTableNameConvention("Entity", string.Empty);
        }
    }
}