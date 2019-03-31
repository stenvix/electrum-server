using Electrum.Identity.Domain;
using Microsoft.EntityFrameworkCore;

namespace Electrum.Identity.Context
{
    public class IdentityContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {

        }
    }
}
