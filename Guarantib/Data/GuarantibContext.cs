using Microsoft.EntityFrameworkCore;

namespace Guarantib.Models
{
    public class GuarantibContext : DbContext
    {
        public GuarantibContext(DbContextOptions<GuarantibContext> options) : base(options)
        {

        }
        public DbSet<Client> clients { get; set; }
        public DbSet<Produit> produits { get; set; }
        public DbSet<Professionnel> professionnels { get; set; }
        public DbSet<Marque> marques { get; set; }
        public DbSet<Suivis> suivis { get; set; }
    }
}
