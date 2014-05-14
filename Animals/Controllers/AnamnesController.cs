using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Animals.Models;

namespace Animals.Controllers
{
    public class AnamnesController : Controller
    {
        private AnimalsEntities db = new AnimalsEntities();

        // GET: /Anamnes/Create
        public ActionResult Create(Guid? petId)
        {
            ViewBag.PetId = petId;

            return View();
        }

        // POST: /Anamnes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Text,PetId,Date")] Anamne anamnes)
        {
            if (ModelState.IsValid)
            {
                anamnes.Id = Guid.NewGuid();
                anamnes.Date = DateTime.Now;
                db.Anamnes.Add(anamnes);
                db.SaveChanges();
                return RedirectToAction("Details","Pets",new { id = anamnes.PetId});
            }

            ViewBag.PetId = new SelectList(db.Pets, "Id", "Nickname", anamnes.PetId);
            return View(anamnes);
        }


        // GET: /Anamnes/Delete/5
        public ActionResult Delete(Guid? id,Guid petId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Anamne anamnes = db.Anamnes.Find(id);
            if (anamnes == null)
            {
                return HttpNotFound();
            }

            ViewBag.petId = petId;

            return View(anamnes);
        }

        // POST: /Anamnes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            string petId = Request["petId"];

            Anamne anamnes = db.Anamnes.Find(id);
            db.Anamnes.Remove(anamnes);
            db.SaveChanges();
            return RedirectToAction("Details", "Pets", new { id = petId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
