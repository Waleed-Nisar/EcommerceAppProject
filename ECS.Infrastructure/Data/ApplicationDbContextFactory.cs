using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ECS.Infrastructure.Data
{
    public class ApplicationDbContextFactory
        : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            optionsBuilder.UseSqlServer(
                "Server=WALEED\\SQLEXPRESS;Database=ECommerceApplication;Trusted_Connection=True;TrustServerCertificate=True;"
            );

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
