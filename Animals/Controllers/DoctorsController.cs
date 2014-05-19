using System;
using System.Linq;
using System.Net;
using Animals.Models;
using System.Web.Mvc;
using Animals.Repository;
using Animals.Extansions;


namespace Animals.Controllers
{
    [Authorize]
    public class DoctorsController : Controller
    {
        private readonly IRepository<Doctor> _doctorRepository;

        public DoctorsController(IRepository<Doctor> docRepository)
        {
            this._doctorRepository = docRepository;
        }

        public ActionResult Index()
        {
            return View(_doctorRepository.FindAll().ToList());
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Doctor doctor = _doctorRepository.Find(id.ToGuid());

            if (doctor == null)
            {
                return HttpNotFound();
            }

            return View(doctor);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Name,Sername,Patronymic,Phone,Date")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                doctor.Id = Guid.NewGuid();
                _doctorRepository.Add(doctor);
                _doctorRepository.SaveAll();
                return RedirectToAction("Index");
            }

            return View(doctor);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Doctor doctor = _doctorRepository.Find(id.ToGuid());
            
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Name,Sername,Patronymic,Phone,Date")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                _doctorRepository.Update(doctor);
                _doctorRepository.SaveAll();

                return RedirectToAction("Index");
            }
            return View(doctor);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Guid idGuid = (Guid)id;
            Doctor doctor = _doctorRepository.Find(idGuid);
            
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Doctor doctor = _doctorRepository.Find(id);
            _doctorRepository.Delete(doctor);
            _doctorRepository.SaveAll();
            
            return RedirectToAction("Index");
        }
    }
}
