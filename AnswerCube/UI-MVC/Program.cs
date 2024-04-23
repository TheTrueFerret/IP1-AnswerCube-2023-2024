using AnswerCube.BL;
using AnswerCube.BL.Domain.User;
using AnswerCube.DAL;
using AnswerCube.DAL.EF;
using AnswerCube.UI.MVC.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
services.AddDbContext<AnswerCubeDbContext>(optionsBuilder =>
    {
        //optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=DataBase IP1 Testssssss;Username=postgres;Password=Student_1234;");
        //optionsBuilder.UseNpgsql("Host=34.79.59.216;Username=postgres;Password=Student_1234;Database=DataBase IP1 Testssssss;");
        optionsBuilder.UseNpgsql(AnswerCubeDbContext.NewPostgreSqlTCPConnectionString().ToString());
    }
);

services.Configure<IdentityOptions>(options =>
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

services.AddDefaultIdentity<AnswerCubeUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AnswerCubeDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();
services.AddScoped<IRepository, Repository>();
services.AddScoped<IManager, Manager>();

// Add services to the container.
services.AddControllersWithViews();
services.AddRazorPages().AddRazorRuntimeCompilation();

services.AddTransient<IEmailSender, MailService>();

// Add Sessions to make sure Models Persist between Controller Requests
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID");
    googleOptions.ClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET");
});

services.AddLogging(logging =>
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

app.UseSession();

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