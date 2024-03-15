using AnswerCube.BL;
using AnswerCube.DAL;
using AnswerCube.DAL.EF;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AnswerCubeDbContext>(optionsBuilder =>
    {
        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=DataBase IP1 Testssssss;User Id=postgres;Password=Student_1234;");
    }
);
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IManager, Manager>();

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.Run();