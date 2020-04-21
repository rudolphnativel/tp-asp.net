using System.ComponentModel.DataAnnotations;

namespace BO
{
    public class Arme
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30,MinimumLength =3)]
        public string Nom { get; set; }
        public int Degats { get; set; }
    }
}