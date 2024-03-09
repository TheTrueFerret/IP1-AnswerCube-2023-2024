// See https://aka.ms/new-console-template for more information

using AnswerCube.BL;
using AnswerCube.DAL.EF;
using AnswerCube.UI.CA;
using Microsoft.EntityFrameworkCore;

var builder = new DbContextOptionsBuilder();
builder.UseNpgsql("Data Source=../../GameAppDataBase.db");
var AnswerCubeContext = new AnswerCubeDbContext(builder.Options);
var repository = new Repository(AnswerCubeContext);
var manager = new Manager(repository);
var test = new CliTest(manager);