using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BO;
using tp1_module_6.Data;
using tp1_module_6.Models;

namespace tp1_module_6.Controllers
{
    public class SamouraisController : Controller
    {
        private tp1_module_6Context db = new tp1_module_6Context();
        public const string LISTE_ARME_VIDE = "zero Arme dans la liste";

        // GET: Samourais
        public ActionResult Index()
        {
            return View(db.Samourais.ToList());
        }

        // GET: Samourais/Details/5
        public ActionResult Details(int? id)
        {
            SamouraisVm vm = new SamouraisVm();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vm.samourai = db.Samourais.Find(id);

            if (vm.samourai == null)
            {
                return HttpNotFound();
            }
           
            return View(vm);
        }

        // GET: Samourais/Create
        public ActionResult Create()
        {
            SamouraisVm vm = new SamouraisVm();
            ChargeArme(vm);

            return View(vm);

        }

        // POST: Samourais/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SamouraisVm vm)
        {
            if (ModelState.IsValid && Verif(vm))
            {

                db.Samourais.Add(vm.samourai);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ChargeArme(vm);
            return View(vm);
        }

        // GET: Samourais/Edit/5
        public ActionResult Edit(int? id)
        {
            SamouraisVm vm = new SamouraisVm();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vm.samourai = db.Samourais.Find(id);
            ChargeArme(vm);
            if (vm.samourai == null || vm.armes == null)
            {
                return RedirectToAction("", "Index", 1);
            }
            return View(vm);
        }

        // POST: Samourais/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, SamouraisVm vm)
        {
            if (vm.samourai.Id == 0 && id != 0)
            {
                vm.samourai.Id = id;
            }
            Samourai samouraidb = new Samourai();
            if (ModelState.IsValid && Verif(vm))
            {
                // db.Entry(vm.samourai).State = EntityState.Modified;
                samouraidb = db.Samourais.Find(id);
                samouraidb.Nom = vm.samourai.Nom;
                samouraidb.Force = vm.samourai.Force;
                samouraidb.ArtMartials = vm.samourai.ArtMartials;
                if (vm.idArmes != null)
                {
                    samouraidb.Arme = vm.samourai.Arme;
                }
                else
                {
                    samouraidb.Arme = null;
                }
                db.Entry(samouraidb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ChargeArme(vm);
            return View(vm);
        }

        // GET: Samourais/Delete/5
        public ActionResult Delete(int? id)
        {
            SamouraisVm vm = new SamouraisVm();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vm.samourai = db.Samourais.Find(id);

            if (vm.samourai == null)
            {
                return HttpNotFound();
            }
            return View(vm);
        }

        // POST: Samourais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Samourai samourai = db.Samourais.Find(id);
            db.Samourais.Remove(samourai);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        //verifie certaines contraintes
        public bool Verif(SamouraisVm vm)
        {
            bool resultat = true;
            if (vm.samourai != null)
            {
                if (db.Samourais.Any(s => s.Nom.ToUpper() == vm.samourai.Nom.ToUpper() && vm.samourai.Id != s.Id))
                {
                    ModelState.AddModelError("samourai.Nom", "Il existe déja un samouraï qui porte le même nom.");
                    resultat = false;
                }
            }
            else
            {
                ModelState.AddModelError("", "Créer d'abord des armes.");
                resultat = false;
            }
            if (vm.samourai.Force < 10)
            {
                ModelState.AddModelError("samourai.Force", "Veuillez octroyer plus de 10 points de force à votre samourai.");
                resultat = false;
            }
            if (vm.idArmes != null && vm.idArmes != 0)
            {//si idArmes  n'est pas null on hydrate  l'objet vm au passage
                
                if (db.Samourais.Any(s => s.Arme.Id == vm.idArmes && vm.samourai.Id != s.Id))
                {
                    ModelState.AddModelError("idArmes", "Il existe déja un samourai qui porte cette arme.");
                    resultat = false;
                }
                else
                {
                    vm.samourai.Arme = db.Armes.Find(vm.idArmes);
                }
            }
            if (vm.idArtMartials!=null )
            {//si la liste de id artMartial n'est pas vide on hydrate l'objet vm au passage
                foreach (var item in vm.idArtMartials)
                {
                    ArtMartial artMartial = db.ArtMartials.Find(item);
                    vm.samourai.ArtMartials.Add(artMartial);
                }
            }


            return resultat;
        }


        public void ChargeArme(SamouraisVm vm)
        {
           
            foreach (var item in db.Armes.ToList())
            {
                if (!db.Samourais.Any(s => s.Arme.Id == item.Id && vm.samourai.Id != s.Id))
                {
                    vm.armes.Add(item);
                }
            }
            
            vm.artMartials = db.ArtMartials.ToList();
            if (vm.armes.Count < 1)
            {
                ModelState.AddModelError("", "Créer d'abord de nouvelles armes.");

            }
        }
    }
}
