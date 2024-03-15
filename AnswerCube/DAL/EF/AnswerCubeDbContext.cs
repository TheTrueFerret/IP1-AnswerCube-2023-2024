using System.Diagnostics;
using AnswerCube.BL.Domain.Slide;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace AnswerCube.DAL.EF;

public class AnswerCubeDbContext : DbContext
{
    //public DbSet<Project> Projects { get; set; }
    //public DbSet<Organization> Organizations { get; set; }
    public DbSet<Info> InfoSlide { get; set; }
    public DbSet<List_Question> ListQuestions { get; set; }
    public DbSet<Open_Question> OpenQuestions { get; set; }
    public DbSet<Requesting_Data> RequestingData { get; set; }

    //TODO: add dbsets if needed

    public AnswerCubeDbContext(DbContextOptions options) : base(options)
    {
        AnswerCubeInitializer.Initialize(this, true);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            //optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=DataBase IP1 Testssssss;User Id=postgres;Password=Student_1234;");
            optionsBuilder.UseNpgsql(NewPostgreSqlTCPConnectionString().ToString());
        }
        optionsBuilder.LogTo(message => Debug.WriteLine(message), LogLevel.Information);
    }
    
    public static NpgsqlConnectionStringBuilder NewPostgreSqlTCPConnectionString()
    {
        var connectionString = new NpgsqlConnectionStringBuilder()
        {
            Host = Environment.GetEnvironmentVariable("INSTANCE_HOST"),     // e.g. '127.0.0.1'
            Username = Environment.GetEnvironmentVariable("DB_USER"), // e.g. 'my-db-user'
            Password = Environment.GetEnvironmentVariable("DB_PASS"), // e.g. 'my-db-password'
            Database = Environment.GetEnvironmentVariable("DB_NAME"), // e.g. 'my-database'

            // The Cloud SQL proxy provides encryption between the proxy and instance.
            SslMode = SslMode.Disable,
        };
        connectionString.Pooling = true;
        // Specify additional properties here.
        return connectionString;
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //TODO: add modelbuilder (relations, required, etc.)
    }
}