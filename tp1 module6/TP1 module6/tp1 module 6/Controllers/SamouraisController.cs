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
            
            if (vm.samourai == null  )
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
            if ( vm.armes == null)
            {
                return HttpNotFound();
            }
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
                vm.samourai.Arme = db.Armes.Find(vm.idArmes);
                 
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
            if (vm.samourai == null ||vm.armes == null)
            {
                return HttpNotFound();
            }
            return View(vm);
        }

        // POST: Samourais/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( int id ,SamouraisVm vm)
        {
            if (vm.samourai.Id == 0 && id!=0 )
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
                samouraidb.Arme = vm.samourai.Arme;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vm.samourai);
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
        public bool Verif (SamouraisVm vm)
        {
            bool resultat = true;
            if (vm.samourai!=null)
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
            if (vm.samourai.Force<10)
            {
                ModelState.AddModelError("samourai.Force", "Veuillez octroyer plus de 10 points de force à votre samourai.");
                resultat = false;
            }
            
            return resultat;
        }
        public void ChargeArme (SamouraisVm vm)
        {
            vm.armes = db.Armes.Select(
                x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                .ToList();
            if (vm.armes.Count<1)
            {
                ModelState.AddModelError("", "Créer d'abord des armes.");
            }
        }
    }
}
