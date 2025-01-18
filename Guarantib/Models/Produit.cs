using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Guarantib.Models
{
    public class Produit
    {
       
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Brand { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string? NumFile { get; set; }

        public string? NumAkuito { get; set; }

        public string NumSerie { get; set; }

        public bool IsClose { get; set; }

        public string? Description { get; set; }

        public ICollection<Professionnel> Professionnels { get; set; }

        public ICollection<Client> Clients { get; set; }

        public ICollection<Marque> marques { get; set; }

        public ICollection<Suivis> suivis { get; set; }


        [NotMapped]
        public int SelectedProfessionnelId { get; set; }
        [NotMapped]
        public int SelectedClientId { get; set; }

        [NotMapped]
        public int SelectedMarqueId { get; set; }

    }
}
