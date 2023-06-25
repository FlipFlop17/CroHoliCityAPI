using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CroHoliCityAPI.Model
{
    public class Lokacija
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int PostanskiBroj { get; set; }
        /// <summary>
        /// Naziv grada/mjesta
        /// </summary>
        public string Naziv { get; set; }
        public string? Naselje { get; set; }
        public string Zupanija { get; set; }
        public DateTime VrijediOd { get; set; } 
        public DateTime? VrijediDo { get; set; } 

    }


}

