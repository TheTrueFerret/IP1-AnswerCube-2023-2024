using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AnswerCube.DAL.EF;

public class AnswerCubeInitializer
{
    public static void Initialize(AnswerCubeDbContext context, bool dropCreateDatabase = false)
    {
        if (dropCreateDatabase)
        {
            context.Database.EnsureDeleted();
        }

        if (context.Database.EnsureCreated())
        {
            Seed(context);
        }
    }

    private static void Seed(AnswerCubeDbContext context)
    {
        //TODO: add seed data from file
        
        
        context.SaveChanges();
        context.ChangeTracker.Clear();
    }
}