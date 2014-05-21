using System;
using System.Net;
using System.Web.Mvc;
using Animals.Models;
using Animals.Repository;
using Animals.Extansions;

namespace Animals.Controllers
{
    [Authorize]
    public class SurgicalController : Controller
    {
        private readonly IRepository<Surgical_treatment> _surgicalRepository;

        public SurgicalController(IRepository<Surgical_treatment> surgicalRepository)
        {
            this._surgicalRepository = surgicalRepository;
        }

        public ActionResult Create(Guid? petId)
        {
            ViewBag.PetId = petId;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,PetId,Text,Date")] Surgical_treatment surgical_treatment)
        {
            if (ModelState.IsValid)
            {
                surgical_treatment.Id = Guid.NewGuid();
                surgical_treatment.Date = DateTime.Now;
                _surgicalRepository.Add(surgical_treatment);
                _surgicalRepository.SaveAll();
                return RedirectToAction("Details", "Pets", new { id = surgical_treatment.PetId });
            }

            return View(surgical_treatment);
        }

        public ActionResult Delete(Guid? id, Guid petId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Surgical_treatment surgical_treatment = _surgicalRepository.Find(id.ToGuid());
            if (surgical_treatment == null)
            {
                return HttpNotFound();
            }

            ViewBag.petId = petId;

            return View(surgical_treatment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            string petId = Request["petId"];

            Surgical_treatment surgical_treatment = _surgicalRepository.Find(id);

            _surgicalRepository.Delete(surgical_treatment);
            _surgicalRepository.SaveAll();

            return RedirectToAction("Details", "Pets", new { id = petId });
        }
    }
}
