using System.ComponentModel.DataAnnotations.Schema;

namespace Guarantib.Models
{
    public class Marque
    {
        public int Id {  get; set; }

        public string Name { get; set; }

        public string Lien { get; set; }

        public ICollection<Produit> Produits { get; set; }

    }
}
