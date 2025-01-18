namespace Guarantib.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Produit> Produits { get; set; }
    }
}
