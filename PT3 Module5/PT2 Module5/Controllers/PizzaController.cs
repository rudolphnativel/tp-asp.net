using PT2_Module5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PT2_Module5.Controllers
{
    public class PizzaController : Controller
    {

        // GET: Pizza
        public ActionResult Index()
        {

            return View(SingletonListPizza.Instance.listPizza);
        }

        // GET: Pizza/Details/5
        public ActionResult Details(int? id)
        {
            Pizza pizza = new Pizza();
            pizza = SingletonListPizza.Instance.listPizza.Where(p => p.Id == id).FirstOrDefault();
            return View(pizza);
        }

        // GET: Pizza/Create 
        public ActionResult Create()
        {
            PizzaVm vm = new PizzaVm();
            vm.listePate = Singleton.Instance.listePate.Select(
                x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                .ToList();
            vm.listeIngredient = Singleton.Instance.listeIngredient.Select(
               x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
               .ToList();
            if (vm.listeIngredient != null && vm.listePate != null)
            {
                return View(vm);
            }
            else return RedirectPermanent("Index");
        }
        private bool ValidateVM(PizzaVm vm)
        {
            bool result = true;
            if (vm.IdsIngredients.Count()<2)
            {
                result = false;
                ModelState.AddModelError("", "Il vous faut une sélection de deux ingrédients au minimun.");
            }
            else if (vm.IdsIngredients.Count() > 5)
            {
                result = false;
                ModelState.AddModelError("", "Il vous faut une sélection de cinq ingrédients au maximun.");
            }
            if (SingletonListPizza.Instance.listPizza.Any(p => p.Nom.ToUpper() == vm.pizza.Nom.ToUpper()&& vm.pizza.Id!=p.Id))
            {
                result = false;
                ModelState.AddModelError("", "Il existe déja une pizza qui porte ce nom.");
            }

            
            return result;
        }
        // POST: Pizza/Create
        [HttpPost]
        public ActionResult Create(PizzaVm vm)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    if (ValidateVM(vm))
                    {
                        Pizza pizza = vm.pizza;

                        pizza.Pate = Singleton.Instance.listePate.FirstOrDefault(x => x.Id == vm.IdPate);

                        pizza.Ingredients = Singleton.Instance.listeIngredient.Where(
                            x => vm.IdsIngredients.Contains(x.Id))
                            .ToList();

                        // Insuffisant
                        //pizza.Id = FakeDb.Instance.Pizzas.Count + 1;

                        pizza.Id = SingletonListPizza.Instance.listPizza.Count == 0 ? 1 : SingletonListPizza.Instance.listPizza.Max(x => x.Id) + 1;

                        SingletonListPizza.Instance.listPizza.Add(pizza);

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        vm.listePate = Singleton.Instance.listePate.Select(
               x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
               .ToList();

                        vm.listeIngredient = Singleton.Instance.listeIngredient.Select(
                            x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                            .ToList();

                        return View(vm);
                    }
                }
                else
                {

                    vm.listePate = Singleton.Instance.listePate.Select(
                x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                .ToList();

                    vm.listeIngredient = Singleton.Instance.listeIngredient.Select(
                        x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                        .ToList();

                    return View(vm);
                }
            }
            catch
            {
                vm.listePate = Singleton.Instance.listePate.Select(
                x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                .ToList();

                vm.listeIngredient = Singleton.Instance.listeIngredient.Select(
                    x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                    .ToList();

                return View(vm);
            }

        }
        // GET: Pizza/Edit/5
        public ActionResult Edit(int id)
        {
            PizzaVm vm = new PizzaVm();


            vm.listePate = Singleton.Instance.listePate.Select(
                x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                .ToList();
            vm.listeIngredient = Singleton.Instance.listeIngredient.Select(
               x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
               .ToList();
            vm.pizza = SingletonListPizza.Instance.listPizza.FirstOrDefault(x => x.Id == id);
            if (vm.pizza.Pate != null)
            {
                vm.IdPate = vm.pizza.Pate.Id;
            }

            if (vm.pizza.Ingredients.Any())
            {
                vm.IdsIngredients = vm.pizza.Ingredients.Select(x => x.Id).ToList();
            }

            return View(vm);
        }
        // POST: Pizza/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, PizzaVm vm)
        {
            vm.pizza.Id = id;
            if (ModelState.IsValid&& ValidateVM(vm))
            {
                try

                {
                    Pizza pizza = SingletonListPizza.Instance.listPizza.FirstOrDefault(x => x.Id == id);

                    pizza.Nom = vm.pizza.Nom;
                    pizza.Pate = Singleton.Instance.listePate.FirstOrDefault(x => x.Id == vm.IdPate);
                    pizza.Ingredients = Singleton.Instance.listeIngredient.Where(x => vm.IdsIngredients.Contains(x.Id)).ToList();

                    return RedirectToAction("Index");
                }
                catch
                {
                    vm.listePate = Singleton.Instance.listePate.Select(
             x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
             .ToList();
                    vm.listeIngredient = Singleton.Instance.listeIngredient.Select(
                       x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                       .ToList();
                    vm.pizza = SingletonListPizza.Instance.listPizza.FirstOrDefault(x => x.Id == id);
                    if (vm.pizza.Pate != null)
                    {
                        vm.IdPate = vm.pizza.Pate.Id;
                    }

                    if (vm.pizza.Ingredients.Any())
                    {
                        vm.IdsIngredients = vm.pizza.Ingredients.Select(x => x.Id).ToList();
                    }

                    return View(vm);
                }
            }
            vm.listePate = Singleton.Instance.listePate.Select(
            x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
            .ToList();
            vm.listeIngredient = Singleton.Instance.listeIngredient.Select(
               x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
               .ToList();
            vm.pizza = SingletonListPizza.Instance.listPizza.FirstOrDefault(x => x.Id == id);
            if (vm.pizza.Pate != null)
            {
                vm.IdPate = vm.pizza.Pate.Id;
            }

            if (vm.pizza.Ingredients.Any())
            {
                vm.IdsIngredients = vm.pizza.Ingredients.Select(x => x.Id).ToList();
            }

            return View(vm);

        }

        // GET: Pizza/Delete/5
        public ActionResult Delete(int id)
        {
            Pizza pizza = new Pizza();
            pizza = SingletonListPizza.Instance.listPizza.Where(C => C.Id == id).FirstOrDefault();

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
                Pizza pizza = SingletonListPizza.Instance.listPizza.Where(C => C.Id == id).FirstOrDefault();
                SingletonListPizza.Instance.listPizza.Remove(pizza);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
