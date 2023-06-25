using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CroHoliCityAPI.Model
{
    public class Dan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime Datum { get; set; }
        public bool NeradniDan { get; set; }
        public string NazivDan { get; set; }
        public string Opis { get; set; }
        public DateTime VrijediOd { get; set; }=DateTime.Now;
        public DateTime? VrijediDo { get; set; }
    }
}
