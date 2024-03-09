// See https://aka.ms/new-console-template for more information

using AnswerCube.BL;
using AnswerCube.DAL.EF;
using AnswerCube.UI.CA;
using Microsoft.EntityFrameworkCore;

var builder = new DbContextOptionsBuilder();
//TODO: add connection string
//var configuration = new ConfigurationBuilder()
//    .SetBasePath(Directory.GetCurrentDirectory())
//    .AddJsonFile("appsettings.json")
//    .Build();
//builder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
var answerCubeContext = new AnswerCubeDbContext(builder.Options);
var repository = new Repository(answerCubeContext);
var manager = new Manager(repository);
var test = new CliTest(manager);

test.TestData();