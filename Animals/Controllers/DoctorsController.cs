using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using Animals.Models;
using System.Web.Mvc;
using Animals.Repository;

namespace Animals.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly IRepository<Doctor> _doctorRepository;

        public DoctorsController(IRepository<Doctor> docRepository)
        {
            this._doctorRepository = docRepository;
        }

        // GET: /Doctors/
        public ActionResult Index()
        {
            return View(_doctorRepository.FindAll().ToList());
        }

        // GET: /Doctors/Details/5
        public ActionResult Details(Guid? id)
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

        // GET: /Doctors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Doctors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: /Doctors/Edit/5
        public ActionResult Edit(Guid? id)
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

        // POST: /Doctors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: /Doctors/Delete/5
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

        // POST: /Doctors/Delete/5
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
