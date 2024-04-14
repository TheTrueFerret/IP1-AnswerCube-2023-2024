using AnswerCube.BL;
using AnswerCube.BL.Domain.User;
using AnswerCube.DAL;
using AnswerCube.DAL.EF;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AnswerCubeDbContextConnection") ??
                       throw new InvalidOperationException(
                           "Connection string 'AnswerCubeDbContextConnection' not found.");
builder.Services.AddDbContext<AnswerCubeDbContext>(optionsBuilder =>
    {
        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=DataBase IP1 Testssssss;Username=postgres;Password=Student_1234;");
        //optionsBuilder.UseNpgsql("Host=34.79.59.216;Username=postgres;Password=Student_1234;Database=DataBase IP1 Testssssss;");
        //optionsBuilder.UseNpgsql(AnswerCube.DAL.EF.AnswerCubeDbContext.NewPostgreSqlTCPConnectionString().ToString());
    }
);

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = false;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.RequireUniqueEmail = true;
});

builder.Services.AddDefaultIdentity<AnswerCubeUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AnswerCubeDbContext>().AddDefaultTokenProviders().AddDefaultUI();
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IManager, Manager>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Initialize the database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AnswerCubeDbContext>();
    AnswerCubeInitializer.Initialize(context, true);
}

app.MapRazorPages();

app.Run();