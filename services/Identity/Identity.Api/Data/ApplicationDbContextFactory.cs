using Identity.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using OpenIddict.EntityFrameworkCore;

namespace Identity.Api.Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=IdentityDb;Username=postgres;Password=postgres");
        optionsBuilder.UseOpenIddict();
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
