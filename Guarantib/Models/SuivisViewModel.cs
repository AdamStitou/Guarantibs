using System.ComponentModel.DataAnnotations.Schema;

namespace Guarantib.Models
{
    [NotMapped]   
    public class SuivisViewModel
    {
        public List<Guarantib.Models.Suivis> ListeSuivis { get; set; }
        public Guarantib.Models.Suivis NouveauSuivi { get; set; }
    }
}
