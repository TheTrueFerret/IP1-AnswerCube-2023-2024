using System.Net;
using System.Text;
using AnswerCube.BL;
using AnswerCube.BL.Domain.User;
using AnswerCube.DAL;
using AnswerCube.DAL.EF;
using AnswerCube.UI.MVC.Services;
using AnswerCube.UI.MVC.Services.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
services.AddDbContext<AnswerCubeDbContext>(optionsBuilder =>
    {
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

services.AddIdentity<AnswerCubeUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AnswerCubeDbContext>()
    .AddDefaultTokenProviders();
//    .AddDefaultUI();
//ADD ALL MANAGERS
services.AddScoped<IFlowManager, FlowManager>();
services.AddScoped<IOrganizationManager, OrganizationManager>();
services.AddScoped<IInstallationManager, InstallationManager>();
services.AddScoped<IForumManager, ForumManager>();
services.AddScoped<IMailManager, MailManager>();
services.AddScoped<IAnswerManager, AnswerManager>();
//ADD ALL REPOSITORYS
services.AddScoped<IFlowRepository, FlowRepository>();
services.AddScoped<IOrganizationRepository, OrganizationRepository>();
services.AddScoped<IInstallationRepository, InstallationRepository>();
services.AddScoped<IForumRepository, ForumRepository>();
services.AddScoped<IMailRepository, MailRepository>();
services.AddScoped<IAnswerRepository, AnswerRepository>();
services.AddScoped<JwtService>();
services.AddScoped<UnitOfWork>();


services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });

// Add services to the container.
services.AddControllersWithViews();
services.AddRazorPages().AddRazorRuntimeCompilation();

services.AddTransient<IEmailSender, MailService>();

// Add Sessions to make sure Models Persist between Controller Requests
services.AddDistributedMemoryCache();
services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

services.AddHttpContextAccessor();
services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();



services.AddSingleton<CloudStorageService>();
if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")=="Production")
{
    string REDISCONNECT = Environment.GetEnvironmentVariable("REDIS_HOST") + ":" + Environment.GetEnvironmentVariable("REDIS_PORT");
    Console.WriteLine(REDISCONNECT);
    var redis = ConnectionMultiplexer.Connect(REDISCONNECT);
    services
        .AddDataProtection()
        .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");
    builder.Services.AddSignalR().AddStackExchangeRedis(REDISCONNECT, options =>
    {
        options.Configuration.ChannelPrefix = "SignalR";
        options.Configuration.KeepAlive = 10;
    });
    var redisConfigurationOptions = ConfigurationOptions.Parse(REDISCONNECT);
    builder.Services.AddStackExchangeRedisCache(redisCacheConfig =>
    { 
        redisCacheConfig.ConfigurationOptions = redisConfigurationOptions;
        redisConfigurationOptions.ChannelPrefix = new RedisChannel("session", RedisChannel.PatternMode.Auto);
    });
    builder.Services.AddSession(options =>
    {
        options.Cookie.Name = "answerCubeSession";
        options.IdleTimeout = TimeSpan.FromMinutes(60 * 24);
    });
}
else
{
    services.AddSignalR();
}


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

services.AddScoped<FlowHub>();

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
app.MapHub<FlowHub>("/flowHub");

app.UseRouting();
if (Environment.GetEnvironmentVariable("ENVIRONMENT") == "Production")
{
    app.UseSession();
}

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