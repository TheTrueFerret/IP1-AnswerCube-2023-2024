using System.Diagnostics;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AnswerCube.DAL.EF;

public class AnswerCubeDbContext : DbContext
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    //TODO: add dbsets if needed

    public AnswerCubeDbContext(DbContextOptions options) : base(options)
    {
        AnswerCubeInitializer.Initialize(this);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Data Source=../../GameAppDataBase.db");
        }

        optionsBuilder.LogTo(message => Debug.WriteLine(message), LogLevel.Information);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //TODO: add modelbuilder (relations, required, etc.)
    }
}