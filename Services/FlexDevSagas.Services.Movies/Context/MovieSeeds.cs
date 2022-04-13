using FlexDevSagas.Services.Movies.Entities;

namespace FlexDevSagas.Services.Movies.Context
{
    public class MovieSeeds
    {
        public static void Seed(MovieContext context)
        {
            var movie1 = new Movie()
            {
                Name = "Test 1"
            };
            var movie2 = new Movie()
            {
                Name = "Test 2"
            };
            context.SaveChanges();
        }
    }
}
