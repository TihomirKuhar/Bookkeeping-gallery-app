using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KnjiznicaProjekt.Models
{
    public class Knjiga
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Naziv moze imati maksimalno 50 znakova")]
        public string Naziv { get; set; }
        [Range(1454, 2018, ErrorMessage = "Godina mora bitit između 1454 i 2018")]
        public int Godina { get; set; }
        public int AutorId { get; set; }
        [Display(Name = "Autor")]
        public Autor Autor { get; set; }
    }
}
