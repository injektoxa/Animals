using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Animals.Models;

namespace Animals.Web.Controllers
{
    public class AnamnesController : Controller
    {
        private AnimalsEntities db = new AnimalsEntities();

        // GET: /Anamnes/
        public ActionResult Index()
        {
            var anamnes = db.Anamnes.Include(a => a.Pet);
            return View(anamnes.ToList());
        }

        // GET: /Anamnes/Details/5
        public ActionResult Details(Guid? id)
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
            return View(anamnes);
        }

        // GET: /Anamnes/Create
        public ActionResult Create()
        {
            ViewBag.PetId = new SelectList(db.Pets, "Id", "Nickname");
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
                db.Anamnes.Add(anamnes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PetId = new SelectList(db.Pets, "Id", "Nickname", anamnes.PetId);
            return View(anamnes);
        }

        // GET: /Anamnes/Edit/5
        public ActionResult Edit(Guid? id)
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
            ViewBag.PetId = new SelectList(db.Pets, "Id", "Nickname", anamnes.PetId);
            return View(anamnes);
        }

        // POST: /Anamnes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Text,PetId,Date")] Anamne anamnes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(anamnes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PetId = new SelectList(db.Pets, "Id", "Nickname", anamnes.PetId);
            return View(anamnes);
        }

        // GET: /Anamnes/Delete/5
        public ActionResult Delete(Guid? id)
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
            return View(anamnes);
        }

        // POST: /Anamnes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Anamne anamnes = db.Anamnes.Find(id);
            db.Anamnes.Remove(anamnes);
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
    }
}
