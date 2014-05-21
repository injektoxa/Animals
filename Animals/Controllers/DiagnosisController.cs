using System;
using System.Net;
using System.Web.Mvc;
using Animals.Models;
using Animals.Repository;
using Animals.Extansions;

namespace Animals.Controllers
{
    [Authorize]
    public class DiagnosisController : Controller
    {
         private readonly IRepository<Diagnostic> _diagnosisRepository;

         public DiagnosisController(IRepository<Diagnostic> diagnosisRepository)
         {
            this._diagnosisRepository = diagnosisRepository;
         }

         public ActionResult Create(Guid? petId)
         {
            ViewBag.PetId = petId;
           
             return View();
         }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,PetId,Date,Text")] Diagnostic diagnostic)
        {
            if (ModelState.IsValid)
            {
                diagnostic.Id = Guid.NewGuid();
                diagnostic.Date = DateTime.Now;

                _diagnosisRepository.Add(diagnostic);
                _diagnosisRepository.SaveAll();

                return RedirectToAction("Details", "Pets", new { id = diagnostic.PetId });
            }
            
            return View(diagnostic);
        }

        public ActionResult Delete(Guid? id,Guid petId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Diagnostic diagnostic = _diagnosisRepository.Find(id.ToGuid());
            if (diagnostic == null)
            {
                return HttpNotFound();
            }

            ViewBag.petId = petId;

            return View(diagnostic);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            string petId = Request["petId"];

            Diagnostic anamnes = _diagnosisRepository.Find(id);

            _diagnosisRepository.Delete(anamnes);
            _diagnosisRepository.SaveAll();

            return RedirectToAction("Details", "Pets", new { id = petId });
        }
    }
}
