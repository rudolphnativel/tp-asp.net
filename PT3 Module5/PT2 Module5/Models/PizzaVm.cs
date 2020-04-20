using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PT2_Module5.Models
{
    public class PizzaVm
    {
        public Pizza pizza{ get; set; }
        public List<SelectListItem> listeIngredient { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> listePate { get; set; } = new List<SelectListItem>();

        [Required]
        public int? IdPate { get; set; }
        [Required(ErrorMessage = "Veuillez sélectioner entre 2 et 5 ingrédients.")]
         //[MinLength(2, ErrorMessage = "Vous ne devez pas mettre moins de 2 ingrèdients ")]
         //[MaxLength(5,ErrorMessage ="Vous ne devez pas mettre plus de 5 ingrèdients ")]

        public List<int> IdsIngredients { get; set; } = new List<int>();

    }
}