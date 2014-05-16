using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Animals.Models;
using Animals.ViewModels;

namespace Animals.Controllers
{
    public class PetsController : Controller
    {

        private AnimalsEntities db = new AnimalsEntities();

        // GET: /Pets/Create
        public ActionResult Create(Guid? id)
        {
            PetsVM petVm = new PetsVM();
            petVm.Pet = new Pet();
            petVm.Pet.OwnerId = (Guid)id;  

            Populater populate = new Populater();
            petVm.PetTypes = populate.PopulatePetTypesList();
            petVm.ListDoctors = populate.PopulateDoctorsList(db);

            return View(petVm);
        }

        // POST: /Pets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Pet,ListDoctors,PetTypes")] PetsVM petvm)
        {
           string petType = ((string[])(petvm.PetTypes))[0];
            Guid doctorId = new Guid(((string[])(petvm.ListDoctors))[0]);

            if (petType != "--Выберите тип--" && Guid.Empty != doctorId)
            {
                if (ModelState.IsValid)
                {
                    Pet pet = petvm.Pet;
                    pet.Id = Guid.NewGuid();
                    pet.Date = DateTime.Now;
                    pet.PType = petType;
                    pet.DoctorId = doctorId;
                    pet.OwnerId = petvm.Pet.OwnerId;
                    db.Pets.Add(pet);
                    db.SaveChanges();

                    return RedirectToAction("Details", "Pets", new { id = pet.Id });
                }
            }

            //ViewBag.DoctorId = new SelectList(db.Doctors, "Id", "Name", pet.DoctorId);
            //ViewBag.OwnerId = new SelectList(db.Owners, "Id", "Name", pet.OwnerId);
            return View(petvm);
        }


        public void Capture()
        {
            var stream = Request.InputStream;
            string dump;

            using (var reader = new StreamReader(stream))
                dump = reader.ReadToEnd();

            Pet p = new Pet();
            p.Photos.Add(new Photo());


            var path = Server.MapPath("~/test.jpg");
            System.IO.File.WriteAllBytes(path, String_To_Bytes2(dump));
        }

        // GET: /Pets/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pet pet = db.Pets.Find(id);
            if (pet == null)
            {
                return HttpNotFound();
            }
            
            return View(pet);
        }

        private byte[] String_To_Bytes2(string strInput)
        {
            int numBytes = (strInput.Length) / 2;
            byte[] bytes = new byte[numBytes];

            for (int x = 0; x < numBytes; ++x)
            {
                bytes[x] = Convert.ToByte(strInput.Substring(x * 2, 2), 16);
            }

            return bytes;
        }

        // GET: /Pets/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pet pets = db.Pets.Find(id);
            if (pets == null)
            {
                return HttpNotFound();
            }

            ViewBag.DoctorId = new SelectList(db.Doctors, "Id", "Name", pets.DoctorId);

            return View(pets);
        }

        // POST: /Pets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nickname,PType,Species,Age,Gender,Castration,Vaccination,Deworming,Treatment__parasites,OwnerId,DoctorId,Date,OwnerId,BirthDate")] Pet pet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = pet.Id });
            }
            ViewBag.DoctorId = new SelectList(db.Doctors, "Id", "Name", pet.DoctorId);
            ViewBag.OwnerId = new SelectList(db.Owners, "Id", "Name", pet.OwnerId);
            return View(pet);
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
