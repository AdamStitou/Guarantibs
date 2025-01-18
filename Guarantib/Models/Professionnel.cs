using Microsoft.AspNetCore.Identity;

namespace Guarantib.Models
{
    public class Professionnel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string job { get; set; }

        public bool KeepLoggedIn { get; set; }
        public ICollection<Produit> Produits { get; set;}
        public ICollection<Suivis> suivis { get; set; }
    }
}
