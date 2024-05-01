using System.Diagnostics;
using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.Slide;
using AnswerCube.BL.Domain.User;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace AnswerCube.DAL.EF;

public class AnswerCubeDbContext : IdentityDbContext<AnswerCubeUser>
{
    //public DbSet<Project> Projects { get; set; }
    //public DbSet<Organization> Organizations { get; set; }
    public DbSet<Flow> Flows { get; set; }
    public DbSet<SlideList> SlideLists { get; set; }
    public DbSet<Slide> Slides { get; set; }
    public DbSet<SlideConnection> SlideConnections { get; set; }
    public DbSet<SubTheme> SubThemes { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<Installation> Installations { get; set; }
    public DbSet<AnswerCubeUser> AnswerCubeUsers { get; set; }
    public DbSet<DeelplatformbeheerderEmail> DeelplatformbeheerderEmails { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<UserOrganization> UserOrganizations { get; set; }


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
            Host = Environment.GetEnvironmentVariable("INSTANCE_HOST"), // e.g. '127.0.0.1'
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


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // relation between Flow and SlideList
        builder.Entity<Flow>()
            .HasMany(f => f.SlideList)
            .WithOne(sl => sl.Flow);
        
        builder.Entity<SlideList>()
            .HasOne(sl => sl.Flow)
            .WithMany(f => f.SlideList);
        
        
        // relation between Subtheme and SlideList
        builder.Entity<SubTheme>()
            .HasMany(st => st.SlideList)
            .WithOne(sl => sl.SubTheme);
        
        builder.Entity<SlideList>()
            .HasOne(sl => sl.SubTheme)
            .WithMany(st => st.SlideList);


        // relation between Answer and Slide
        builder.Entity<Answer>()
            .HasOne(a => a.Slide)
            .WithMany(s => s.Answers);
        
        builder.Entity<Slide>()
            .HasMany(s => s.Answers)
            .WithOne(a => a.Slide);
        

        builder.Entity<UserOrganization>()
            .HasKey(uo => new { uo.UserId, uo.OrganizationId });

        builder.Entity<UserOrganization>()
            .HasOne(uo => uo.User)
            .WithMany(u => u.UserOrganizations)
            .HasForeignKey(uo => uo.UserId);

        builder.Entity<UserOrganization>()
            .HasOne(uo => uo.Organization)
            .WithMany(o => o.UserOrganizations)
            .HasForeignKey(uo => uo.OrganizationId);


        SeedRoles(builder);
    }

    private void SeedRoles(ModelBuilder builder)
    {
        builder.Entity<IdentityRole>().HasData
        (
            new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
            new IdentityRole { Name = "Organization", NormalizedName = "ORGANIZATION" },
            new IdentityRole { Name = "DeelplatformBeheerder", NormalizedName = "DEELPLATFORMBEHEERDER" },
            new IdentityRole { Name = "Supervisor", NormalizedName = "SUPERVISOR" },
            new IdentityRole { Name = "Gebruiker", NormalizedName = "GEBRUIKER" }
        );
    }
}