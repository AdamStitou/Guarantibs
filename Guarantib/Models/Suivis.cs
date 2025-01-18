using System.ComponentModel.DataAnnotations.Schema;

namespace Guarantib.Models
{
    public class Suivis
    {
        public int Id { get; set; }

        public string? Etape { get; set; }

        public DateTime DateSuivis { get; set; }

        public string dialogue { get; set; }

        public ICollection<Professionnel> Professionnels { get; set; }

        public ICollection<Produit> Produits { get; set; }

        [NotMapped]
        public int SelectedProfessionnelId { get; set; }
        [NotMapped]
        public int? SelectedProduitId { get; set; }

    }
}
