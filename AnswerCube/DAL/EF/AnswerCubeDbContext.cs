using System.Diagnostics;
using AnswerCube.BL.Domain;
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
    public DbSet<LinearFlow> LinearFlows { get; set; }
    public DbSet<CircularFlow> CircularFlows { get; set; }
    public DbSet<SlideList> SlideLists { get; set; }
    public DbSet<SubTheme> SubThemes { get; set; }
    
    public DbSet<Info> InfoSlide { get; set; }
    public DbSet<ListQuestion> ListQuestions { get; set; }
    public DbSet<OpenQuestion> OpenQuestions { get; set; }
    public DbSet<RequestingInfo> RequestingInfo { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<Installation> Installations { get; set; }
    
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
        base.OnModelCreating(modelBuilder);
        
        // relation between Slide and SlideList
        modelBuilder.Entity<AbstractSlide>()
            .HasOne(s => s.SlideList)
            .WithMany(sl => sl.Slides);
        
        modelBuilder.Entity<SlideList>()
            .HasMany(sl => sl.Slides)
            .WithOne(s => s.SlideList);


        // relation between Flow and SlideList
        modelBuilder.Entity<Flow>()
            .HasMany(f => f.SlideList)
            .WithOne(sl => sl.Flow);
        
        modelBuilder.Entity<SlideList>()
            .HasOne(sl => sl.Flow)
            .WithMany(f => f.SlideList);
        
        
        // relation between Subtheme and SlideList
        modelBuilder.Entity<SubTheme>()
            .HasMany(st => st.SlideList)
            .WithOne(sl => sl.SubTheme);
        
        modelBuilder.Entity<SlideList>()
            .HasOne(sl => sl.SubTheme)
            .WithMany(st => st.SlideList);


        // relation between Answer and Slide
        modelBuilder.Entity<Answer>()
            .HasOne(a => a.AbstractSlide)
            .WithMany(s => s.Answers);
        
        modelBuilder.Entity<AbstractSlide>()
            .HasMany(s => s.Answers)
            .WithOne(a => a.AbstractSlide);
        
        
        // relation between Installation and Flow
        modelBuilder.Entity<Installation>()
            .HasOne(i => i.Flow)
            .WithMany(f => f.ActiveInstallations);
        
        modelBuilder.Entity<Flow>()
            .HasMany(s => s.ActiveInstallations)
            .WithOne(i => i.Flow);
        
        //TODO: add modelbuilder (relations, required, etc.)
    }
}