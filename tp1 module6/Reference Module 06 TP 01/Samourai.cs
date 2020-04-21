using System.ComponentModel.DataAnnotations;

namespace BO
{
    public class Samourai
    {
        public int Id { get; set; }
        [Required]
        public int Force { get; set; }
        [Required]
        [StringLength(25, MinimumLength = 5)]
        public string Nom { get; set; }
        public virtual Arme Arme { get; set; }
    }
}
