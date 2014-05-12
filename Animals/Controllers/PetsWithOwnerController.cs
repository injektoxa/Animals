using Animals.Models;
using Animals.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Animals.Controllers
{
    public class PetsWithOwnerController : Controller
    {
        private AnimalsEntities db = new AnimalsEntities();

        //
        // GET: /AddPetsWithOwner/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /AddPetsWithOwner/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /AddPetsWithOwner/Create
        public ActionResult Create()
        {
            List<string> petTypes = new List<string>(){
            "Собака",
            "Кошка",
            "Хомяк",
            "Шиншила",
            "Харёк",
            "Крыса",
            "Кролик"
            };

            ViewBag.DoctorId = new SelectList(db.Doctors, "Id", "Name");
            ViewBag.PType = new SelectList(petTypes);

            return View();
        }

        //
        // POST: /AddPetsWithOwner/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientsWithPetsVM clientAndPet)
        {

            
            try
            {
                if (ModelState.IsValid)
                {
                    Owner owner = clientAndPet.Owner;

                    owner.Id = Guid.NewGuid();
                    owner.Date = DateTime.Now;
                    db.Owners.Add(owner);
                    db.SaveChanges();

                    Pet pet = clientAndPet.Pet;
                    pet.OwnerId = owner.Id;
                    pet.Id = Guid.NewGuid();
                    pet.Date = DateTime.Now;

                    db.Pets.Add(pet);
                    db.SaveChanges();

                    return RedirectToAction("Index", "Owners");
                }

                return RedirectToAction("Index", "Owners");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /AddPetsWithOwner/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /AddPetsWithOwner/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /AddPetsWithOwner/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /AddPetsWithOwner/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
