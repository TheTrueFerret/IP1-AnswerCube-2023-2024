// See https://aka.ms/new-console-template for more information

using AnswerCube.BL;
using AnswerCube.DAL.EF;
using AnswerCube.UI.CA;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = new DbContextOptionsBuilder();
//TODO: add connection string
//var configuration = new ConfigurationBuilder()
//    .AddJsonFile("UI-CA/appsettings.json")
//    .Build();

builder.UseNpgsql("Host=localhost;Port=5432;Database=DataBase IP1 Testssssss;User Id=postgres;Password=Student_1234;");

var answerCubeContext = new AnswerCubeDbContext(builder.Options);
var repository = new Repository(answerCubeContext);
var manager = new Manager(repository);
var test = new CliTest(manager);

test.TestData();
