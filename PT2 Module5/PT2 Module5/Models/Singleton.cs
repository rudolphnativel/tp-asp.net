using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PT2_Module5.Models
{
    public class Singleton
    {
        /**
         * Class qui représente un e liste de pate, d'ingrédients, et une pizza pour faire la création et la modification des pizza
         * L'objet pizza qui se trouve dans cette class sera utilisé pour remplir la fausse base de données "SingletonListPizza".
         */
        private static Singleton _instance;
        static readonly object instanceLock = new object();
        public List<Ingredient> listeIngredient { get; set; }
        public List<Pate> listePate { get; set; }
        public Pizza pizza { get; set; }
        private Singleton()
        {
            listeIngredient = Pizza.IngredientsDisponibles;
            listePate= Pizza.PatesDisponibles;
            pizza = new Pizza();
        }

        public static Singleton Instance
        {
            get
            {
                if (_instance == null) //Les locks prennent du temps, il est préférable de vérifier d'abord la nullité de l'instance.
                {
                    lock (instanceLock)
                    {
                        if (_instance == null) //on vérifie encore, au cas où l'instance aurait été créée entretemps.
                            _instance = new Singleton();
                    }
                }
                return _instance;
            }
        }
    }
}
