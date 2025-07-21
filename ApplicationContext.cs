using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Test_Task_Datetime.Scheme;

namespace Test_Task_Datetime
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Values> Values => Set<Values>();
        public DbSet<FileNames> FileNames => Set<FileNames>();
        public DbSet<Scheme.Results> Results => Set<Scheme.Results>();
        public ApplicationContext() => Database.EnsureCreated();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var host = "c-c9qa45e72ima621168rh.rw.mdb.yandexcloud.net";
            var port = "6432";
            var db = "datetimedb";
            var username = "postgres_admin";
            var password = "***";
            var connString = $"Host={host};Port={port};Database={db};Username={username};Password={password};Ssl Mode=VerifyFull;Root Certificate=C:/Users/Таня/.postgresql/root.crt";

            //AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true); //apply timestamps

            optionsBuilder.UseNpgsql(connString);
        }
    }
}
