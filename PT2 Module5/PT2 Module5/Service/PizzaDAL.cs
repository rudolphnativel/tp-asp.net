using PT2_Module5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PT2_Module5.Service
{
    public class PizzaDAL
    {
        public static Singleton singleListe = Singleton.Instance;//pour remplir mes listes de pates et d'ingrédients
        public static SingletonListPizza fakeDBPizza = SingletonListPizza.Instance;//la fausse base de données
        public Pizza FindPizza(int? id)
        {
            Pizza pizza = new Pizza();
            if (id != null & id != 0)
            {
                pizza = fakeDBPizza.listPizza.Where(p => p.Id == id).FirstOrDefault();
            }
            else
            {
                return null;
            }
            return pizza;
        }
        public Pizza AddFakeDB(FormCollection collection)

        {

            string test = "";
            Pizza pizza = new Pizza();
            int intTempoId = 0;
            try

            {
                // TODO: Add insert logic here
                foreach (string item in collection)
                {

                    //remplissage de l'objet pizza
                    switch (item)
                    {
                        case "pizza.Nom":
                            //attribu un nom de pizza
                            if (collection[item] != null && collection[item].Trim() != "")
                            {
                                pizza.Nom = collection[item];
                            }


                            break;
                        case "listePate":
                            //attribu un type de pate

                            if (Int32.TryParse(collection[item], out intTempoId))
                            {
                                pizza.Pate = singleListe.listePate.Where(p => p.Id == intTempoId).FirstOrDefault();
                            }


                            break;
                        case "listeIngredient":
                            //attribu des ingrédients
                            if (collection[item] != null && collection[item].Trim() != "")
                            {
                                string ingredientPizza = collection[item];
                                for (int i = 0; i < ingredientPizza.Length; i++)
                                {
                                    if (ingredientPizza[i] != ',')
                                    {
                                        intTempoId = 0;
                                        if (Int32.TryParse(ingredientPizza[i].ToString(), out intTempoId))
                                        {
                                            pizza.Ingredients.Add(singleListe.listeIngredient.Where(f => f.Id == intTempoId).FirstOrDefault());
                                        }
                                    }
                                }
                            }

                            break;
                        default:
                            break;
                    }
                }




                //on lui attribu un id
                int intTempo = 0;
                if (fakeDBPizza.listPizza.Count() > 0)
                {
                    var test2 = fakeDBPizza.listPizza.Max(p => p.Id);
                    if (int.TryParse(test, out intTempo))
                    {
                        pizza.Id = intTempo + 1;
                    }
                }
                else
                {
                    intTempo = 0;
                    pizza.Id = intTempo + 1;
                }


            }
            catch
            {
                return null;
            }
            return pizza;
        }
        public string BuildMessage(Pizza pizza)
        {
            string message = "";

            if (pizza.Ingredients.Count() <= 0)
            {
                message += "Erreur sur saisie ingrédient.";
            }
            if (pizza.Nom == null || pizza.Nom.Trim() == "")
            {
                message += "Erreur sur saisie Nom de la pizza.";
            }
            if (pizza.Pate == null)
            {
                message += "Erreur sur saisie type de pate à pizza.";
            }
            return message;
        }
    }
}
