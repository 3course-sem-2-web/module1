using webapi.SharedServices.Db;

namespace webapi.StudentFeatures
{
    public static class StudentsSeedProvider
    {
        public static void Seed(AppDbContext context)
        {
            context.Database.EnsureCreated();

            var root = new Student
            {
                StudentId = Guid.NewGuid(),
                Nickname = "entsmorphinum",
                Firstname = "Maksym",
                Lastname = "Khomenko"
            };

            context.Students.Add(root);
            
            context.SaveChanges();
        }
    }
}
