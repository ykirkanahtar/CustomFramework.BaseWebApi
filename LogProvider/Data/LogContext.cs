using CustomFramework.BaseWebApi.LogProvider.Data.ModelConfigurations;
using CustomFramework.BaseWebApi.LogProvider.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomFramework.BaseWebApi.LogProvider.Data
{
    public class LogContext : DbContext
    {
        public LogContext(DbContextOptions options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new LogModelConfiguration<Log>());
        }
    }
}