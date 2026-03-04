using Microsoft.EntityFrameworkCore;
using videoapi.Models;

namespace videoapi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Video> Videos { get; set; }
        // Add additional entities such as Users, Tags, etc.
    }
}
