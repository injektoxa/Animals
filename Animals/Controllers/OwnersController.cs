using Animals.Models;
using Animals.Repository;
using Animals.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Animals.Extansions;

namespace Animals.Controllers
{
    [Authorize]
    public class OwnersController : Controller
    {
        private readonly IRepository<Owner> _ownerRepository;
        private readonly IRepository<Pet> _petRepository;
        private readonly IRepository<Doctor> _doctorRepository;

        public OwnersController(IRepository<Owner> ownerRepository, IRepository<Pet> petRepository, IRepository<Doctor> docRepository)
        {
            this._ownerRepository = ownerRepository;
            this._petRepository = petRepository;
            this._doctorRepository = docRepository;
        }

        public ActionResult Index(string searchPet, string searchPhone, string searchAddress, string searchSername, string searchBreed, string searchNumber)
        {
            IEnumerable<Owner> allOwners = _ownerRepository.FindAll();

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

        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Owner owner = _ownerRepository.Find(id.ToGuid());

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

            Populater populater = new Populater();

            clientPetsVm.PetTypes = populater.PopulatePetTypesList();
            clientPetsVm.ListDoctors = populater.PopulateDoctorsList(_doctorRepository.FindAll().ToList());

            return View(clientPetsVm);
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

                _ownerRepository.Add(owner);
                _ownerRepository.SaveAll();

                Pet pet = ownerWithClient.Pet;
                pet.OwnerId = owner.Id;
                pet.Id = Guid.NewGuid();
                pet.Date = DateTime.Now;
                pet.PType = petType;
                pet.DoctorId = doctorId;
                _petRepository.Add(pet);
                _petRepository.SaveAll();

                return RedirectToAction("Index", "Owners");
            }

            Populater populater = new Populater();

            ClientsWithPetsVM clientPetsVm = new ClientsWithPetsVM();
            clientPetsVm.PetTypes = populater.PopulatePetTypesList();
            clientPetsVm.ListDoctors = populater.PopulateDoctorsList(_doctorRepository.FindAll().ToList());
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

            Owner owners = _ownerRepository.Find(id.ToGuid());
            
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
        public ActionResult Edit([Bind(Include = "Id,Name,Sername,Patronymic,Adress,Email,Phone,Date,Number")] Owner owner)
        {
            if (ModelState.IsValid)
            {
                _ownerRepository.Update(owner);

                _ownerRepository.SaveAll();
                
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
            
            Owner owner = _ownerRepository.Find(id.ToGuid());

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
            Owner owner = _ownerRepository.Find(id);

            _ownerRepository.Delete(owner);
            _ownerRepository.SaveAll();
          
            return RedirectToAction("Index");
        }
    }
}
