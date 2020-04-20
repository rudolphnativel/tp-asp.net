using PT2_Module5.Models;
using PT2_Module5.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PT2_Module5.Controllers
{
    public class PizzaController : Controller
    {
        public static Singleton singleListe = Singleton.Instance;//pour remplir mes listes de pates et d'ingrédients
        public static SingletonListPizza fakeDBPizza = SingletonListPizza.Instance;//la fausse base de données
        // GET: Pizza
        public ActionResult Index()
        {

            return View(fakeDBPizza.listPizza);
        }

        // GET: Pizza/Details/5
        public ActionResult Details(int? id)
        {
            Pizza pizza = new Pizza();
            pizza = fakeDBPizza.listPizza.Where(p => p.Id == id).FirstOrDefault();
            return View(pizza);
        }

        // GET: Pizza/Create 
        public ActionResult Create()
        {
            if (singleListe.listeIngredient != null && singleListe.listePate != null)
            {
                return View(singleListe);
            }
            else return RedirectPermanent("Index");
        }

        // POST: Pizza/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            string test = "";
            Pizza pizza = new Pizza(); 
            PizzaDAL pizzaDAL = new PizzaDAL();
            pizza = pizzaDAL.AddFakeDB(collection);
           
            try
            {
                //vérif 
              
                test = pizzaDAL.BuildMessage(pizza);
                //fin vérif

                //on voir si il y a eu des erreur
                if (test != null && test.Trim() != "")
                {
                    ViewBag.message = test;
                    //si il y a une erreur on retour sur la vue et on affiche l'erreur
                    return View(singleListe);
                }
                else
                {

                    //on lui attribu un id
                    int intTempo = 0;
                    if (fakeDBPizza.listPizza.Count() > 0)
                    {
                        intTempo = fakeDBPizza.listPizza.Max(p => p.Id);
                        
                            pizza.Id = intTempo + 1;
                        
                    }
                    else
                    {
                        intTempo = 0;
                        pizza.Id = intTempo + 1;
                    }
                    //on inject dans la base
                    fakeDBPizza.listPizza.Add(pizza);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
                throw;
            }

        }
        // GET: Pizza/Edit/5
        public ActionResult Edit(int id)
        {
            PizzaDAL pizza = new PizzaDAL();
            singleListe.pizza = pizza.FindPizza(id);

            if (singleListe.pizza != null)
            {
                return View(singleListe);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        // POST: Pizza/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            Pizza pizza = new Pizza();
            string test = "";
            int idTest = 0;
            try
            {
                PizzaDAL pizzaDAL = new PizzaDAL();
                //remplissage de l'objet pizza
                pizza = pizzaDAL.AddFakeDB(collection);
                //test du bon remplissage
                test = pizzaDAL.BuildMessage(pizza);
                
                if (pizza != null && test == "")
                {
                    //recherche de l"Id pizza
                    idTest = fakeDBPizza.listPizza.Where(p => p.Id == id).Select(p=> p.Id).FirstOrDefault();
                    pizza.Id = idTest;
                    fakeDBPizza.listPizza[idTest-1] = pizza;
                    return RedirectToAction("Index");
                   
                }
                else
                {
                    ViewBag.message = test;
                    singleListe.pizza = pizzaDAL.FindPizza(id);
                    return View(singleListe);
                }
            }
            catch
            {
                return View(singleListe);
            }
        }

        // GET: Pizza/Delete/5
        public ActionResult Delete(int id)
        {
            Pizza pizza = new Pizza();
            PizzaDAL pizzaDAL = new PizzaDAL();
            pizza = pizzaDAL.FindPizza(id);

            if (pizza != null)
            {
                return View(pizza);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // POST: Pizza/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                Pizza pizza = fakeDBPizza.listPizza.Where(C => C.Id == id).FirstOrDefault();
                fakeDBPizza.listPizza.Remove(pizza);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
