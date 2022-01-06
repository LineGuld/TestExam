using Author_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Author_API.DataAccess
{
    public class AuthorDbContext: DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(
                "Data Source = C:/Users/LineK/RiderProjects/EksamensSæt/Library.db");
        }
    }
}