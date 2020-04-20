using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PT2_Module5.Models
{
    public class SingletonListPizza
    {

        /**
         * class qui représente une fausse base de données de avec ça liste de pizza.
         * Elle est rempli par l'objet Singleton
         */
        private static SingletonListPizza _instance;
        static readonly object instanceLock = new object();
       
        public List<Pizza> listPizza { get; set; }
        private SingletonListPizza()
        {
            this.listPizza = new List<Pizza>();
        }
       
        public static SingletonListPizza Instance
        {
            get
            {
                if (_instance == null) //Les locks prennent du temps, il est préférable de vérifier d'abord la nullité de l'instance.
                {
                    lock (instanceLock)
                    {
                        if (_instance == null) //on vérifie encore, au cas où l'instance aurait été créée entretemps.
                            _instance = new SingletonListPizza();
                    }
                }
                return _instance;
            }
        }
    }
}