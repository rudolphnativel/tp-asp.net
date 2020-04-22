using BO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace tp1_module_6.Models
{
    public class SamouraisVm
    {
        public Samourai samourai { get; set; }
        public List<SelectListItem> armes { get; set; } = new List<SelectListItem>();
        [Required(ErrorMessage ="Un samouraï n'est rien sans une arme!")]
        public  int idArmes { get; set; }
    }
}