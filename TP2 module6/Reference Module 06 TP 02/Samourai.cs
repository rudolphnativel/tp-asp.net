using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BO
{
    public class Samourai : Ids
    {
        [Required]
        public int Force { get; set; }
        [Required]
        [StringLength(25, MinimumLength = 5)]
        public string Nom { get; set; }
        public virtual Arme Arme { get; set; }
        public virtual List<ArtMartial> ArtMartials { get; set; } = new List<ArtMartial>();
        public int? potentiel
        {
            get
            {
                int potentiel = this.Force;
                if (this.Arme != null)
                {
                    potentiel += this.Arme.Degats;
                }
                potentiel *= (this.ArtMartials.Count + 1);
                return potentiel;
            }
        }

    }
}
