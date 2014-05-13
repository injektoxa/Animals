using Animals.Models;
using Animals.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Animals.Controllers
{


    //[Authorize]
    public class OwnersController : Controller
    {
        private AnimalsEntities db = new AnimalsEntities();

        // GET: /Owners/
        public ActionResult Index(string searchPet, string searchPhone, string searchAddress, string searchSername, string searchBreed, string searchNumber)
        {
            IEnumerable<Owner> allOwners = db.Owners;

            if (string.IsNullOrEmpty(searchPet)
                && string.IsNullOrEmpty(searchPhone)
                && string.IsNullOrEmpty(searchAddress)
                && string.IsNullOrEmpty(searchNumber)
                && string.IsNullOrEmpty(searchBreed)
                && string.IsNullOrEmpty(searchSername))
            {
                allOwners = FilterDate(allOwners);
            }

            if (!string.IsNullOrEmpty(searchSername))
            {
                allOwners = FilterSername(allOwners, searchSername);
            }

            if (!string.IsNullOrEmpty(searchPhone))
            {
                allOwners = FilterPhone(allOwners, searchPhone);
            }

            if (!string.IsNullOrEmpty(searchAddress))
            {
                allOwners = FilterAddress(allOwners, searchAddress);
            }

            if (!string.IsNullOrEmpty(searchNumber))
            {
                allOwners = FilterNumber(allOwners, searchNumber);
            }

            if (!string.IsNullOrEmpty(searchPet))
            {
                allOwners = FilterPets(allOwners, searchPet);
            }

            if (!string.IsNullOrEmpty(searchBreed))
            {
                allOwners = FilterBreed(allOwners, searchBreed);
            }

            return View(allOwners);
        }

        private IEnumerable<Owner> FilterBreed(IEnumerable<Owner> allOwners, string searchBreed)
        {
            List<Owner> owlist = new List<Owner>();

            foreach (var o in allOwners)
            {
                foreach (var p in o.Pets)
                {
                    if (p.Species.Contains(searchBreed))
                    {
                        owlist.Add(o);
                    }
                }
            }

            return owlist as IEnumerable<Owner>;
        }

        private IEnumerable<Owner> FilterPets(IEnumerable<Owner> allOwners, string searchPet)
        {
            List<Owner> owlist = new List<Owner>();

            foreach (var owner in allOwners)
            {
                foreach (var pet in owner.Pets)
                {
                    if (pet.Nickname.StartsWith(searchPet, StringComparison.InvariantCultureIgnoreCase))
                    {
                        owlist.Add(owner);
                    }
                }
            }

            return owlist as IEnumerable<Owner>;
        }

        private IEnumerable<Owner> FilterNumber(IEnumerable<Owner> allOwners, string searchNumber)
        {
            List<Owner> owlist = new List<Owner>();

            foreach (var s in allOwners)
            {
                if (s.Number.ToString().Contains(searchNumber))
                {
                    owlist.Add(s);
                }
            }

            return owlist as IEnumerable<Owner>;
        }

        private IEnumerable<Owner> FilterAddress(IEnumerable<Owner> allOwners, string searchAddress)
        {
            return allOwners.Where(o => o.Adress.StartsWith(searchAddress, StringComparison.CurrentCultureIgnoreCase));
        }

        private IEnumerable<Owner> FilterPhone(IEnumerable<Owner> allOwners, string searchPhone)
        {
            return allOwners.Where(o => o.Phone.StartsWith(searchPhone));
        }

        private IEnumerable<Owner> FilterSername(IEnumerable<Owner> allOwners, string searchSername)
        {
            return allOwners.Where(o => o.Sername.StartsWith(searchSername, StringComparison.CurrentCultureIgnoreCase));
        }

        private IEnumerable<Owner> FilterDate(IEnumerable<Owner> allOwners)
        {
            DateTime dtMonthAgo = DateTime.Now.AddDays(-30);
            return allOwners.Where(o => o.Date > dtMonthAgo).OrderByDescending(a => a.Number);
        }

        // GET: /Owner/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Owner owner = db.Owners.Find(id);
            if (owner == null)
            {
                return HttpNotFound();
            }

            return View(owner);
        }

        // GET: /Owner/Create
        public ActionResult Create()
        {
            ClientsWithPetsVM clientPetsVm = new ClientsWithPetsVM();
            clientPetsVm.PetTypes = PopulatePetTypesList();
            clientPetsVm.ListDoctors = PopulateDoctorsList();

            return View(clientPetsVm);
        }

        private List<string> PopulatePetTypesList()
        {
            List<string> petTypes;

            return petTypes = new List<string>(){
            "--Выберите тип--",
            "Собака",
            "Кошка",
            "Хомяк",
            "Шиншила",
            "Харёк",
            "Крыса",
            "Кролик"
            };
        }

        private List<DoctorForDDl> PopulateDoctorsList()
        {
            List<DoctorForDDl> doctorsList = new List<DoctorForDDl>();
            DoctorForDDl doctor = new DoctorForDDl();
            doctor.Id = Guid.Empty;
            doctor.Name = "---Выберите доктора---";

            doctorsList.Add(doctor);

            foreach (var i in db.Doctors)
            {
                DoctorForDDl doc = new DoctorForDDl();
                doc.Id = i.Id;
                doc.Name = i.Name;

                doctorsList.Add(doc);
            }

            return doctorsList;
        }

        // POST: /Owner/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Pet,Owner,ListDoctors,PetTypes")] ClientsWithPetsVM ownerWithClient)
        {
            string petType = ((string[])(ownerWithClient.PetTypes))[0];
            Guid doctorId = new Guid(((string[])(ownerWithClient.ListDoctors))[0]);

            if (petType != "--Выберите тип--" && Guid.Empty != doctorId)
            {
                Owner owner = ownerWithClient.Owner;

                owner.Id = Guid.NewGuid();
                owner.Date = DateTime.Now;
                db.Owners.Add(owner);
                db.SaveChanges();

                Pet pet = ownerWithClient.Pet;
                pet.OwnerId = owner.Id;
                pet.Id = Guid.NewGuid();
                pet.Date = DateTime.Now;
                pet.PType = petType;
                pet.DoctorId = doctorId;
                db.Pets.Add(pet);
                db.SaveChanges();

                return RedirectToAction("Index", "Owners");
            }

            ClientsWithPetsVM clientPetsVm = new ClientsWithPetsVM();
            clientPetsVm.PetTypes = PopulatePetTypesList();
            clientPetsVm.ListDoctors = PopulateDoctorsList();
            clientPetsVm.Message = "Доктор или тип животного не выбран";

            return View(clientPetsVm);
        }

        // GET: /Owner/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Owner owners = db.Owners.Find(id);
            if (owners == null)
            {
                return HttpNotFound();
            }
            return View(owners);
        }

        // POST: /Owner/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Sername,Patronymic,Adress,Phone,Date,Number")] Owner owner)
        {
            if (ModelState.IsValid)
            {
                db.Entry(owner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(owner);
        }

        // GET: /Owner/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Owner owner = db.Owners.Find(id);
            if (owner == null)
            {
                return HttpNotFound();
            }
            return View(owner);
        }

        // POST: /Owner/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Owner owner = db.Owners.Find(id);

            var pets = db.Pets.ToList().Where(p => p.OwnerId == id).ToList();

            if (pets != null)
            {
                for (int i = 0; i < pets.Count(); i++)
                {
                    db.Pets.Remove(pets[i]);
                }
            }

            db.Owners.Remove(owner);
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
