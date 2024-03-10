using System.Diagnostics;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=DataBase IP1 Testssssss;User Id=postgres;Password=Student_1234;");
        }

        optionsBuilder.LogTo(message => Debug.WriteLine(message), LogLevel.Information);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //TODO: add modelbuilder (relations, required, etc.)
    }
}