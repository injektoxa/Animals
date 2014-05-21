using System;
using System.Net;
using System.Web.Mvc;
using Animals.Models;
using Animals.Repository;
using Animals.Extansions;

namespace Animals.Controllers
{
    [Authorize]
    public class TreatmentsController : Controller
    {
        private readonly IRepository<Treatment> _treatmentRepository;

        public TreatmentsController(IRepository<Treatment> treatmentRepository)
        {
            this._treatmentRepository = treatmentRepository;
        }

        public ActionResult Create(Guid? petId)
        {
            ViewBag.PetId = petId;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,PetId,Text,Date")] Treatment treatment)
        {
            if (ModelState.IsValid)
            {
                treatment.Id = Guid.NewGuid();
                treatment.Date = DateTime.Now;

                _treatmentRepository.Add(treatment);
                _treatmentRepository.SaveAll();
                return RedirectToAction("Details", "Pets", new { id = treatment.PetId });
            }

            return View(treatment);
        }


        public ActionResult Delete(Guid? id, Guid petId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Treatment treatment = _treatmentRepository.Find(id.ToGuid());
            if (treatment == null)
            {
                return HttpNotFound();
            }

            ViewBag.petId = petId;

            return View(treatment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            string petId = Request["petId"];

            Treatment treatment = _treatmentRepository.Find(id);
            _treatmentRepository.Delete(treatment);
            _treatmentRepository.SaveAll();
            
            return RedirectToAction("Details", "Pets", new { id = petId });
        }
    }
}
