using System;
using System.Net;
using System.Web.Mvc;
using Animals.Models;
using Animals.Repository;
using Animals.Extansions;

namespace Animals.Controllers
{
    [Authorize]
    public class AnamnesController : Controller
    {
        private readonly IRepository<Anamne> _anamnesRepository;

        public AnamnesController(IRepository<Anamne> anamnesRepository)
        {
            this._anamnesRepository = anamnesRepository;
        }

        public ActionResult Create(Guid? petId)
        {
            ViewBag.PetId = petId;

            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Text,PetId,Date")] Anamne anamnes)
        {
            if (ModelState.IsValid)
            {
                anamnes.Id = Guid.NewGuid();
                anamnes.Date = DateTime.Now;
                _anamnesRepository.Add(anamnes);
                _anamnesRepository.SaveAll();
                return RedirectToAction("Details", "Pets", new { id = anamnes.PetId });
            }

            return View(anamnes);
        }

        public ActionResult Delete(Guid? id, Guid petId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Anamne anamnes = _anamnesRepository.Find(id.ToGuid());
            if (anamnes == null)
            {
                return HttpNotFound();
            }

            ViewBag.petId = petId;

            return View(anamnes);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            string petId = Request["petId"];

            Anamne anamnes = _anamnesRepository.Find(id);
            
            _anamnesRepository.Delete(anamnes);
            _anamnesRepository.SaveAll();
            
            return RedirectToAction("Details", "Pets", new { id = petId });
        }
    }
}
