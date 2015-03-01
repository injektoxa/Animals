using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Net;
using System.Transactions;
using System.Web.Mvc;
using Animals.Extansions;
using Animals.Models;
using Animals.Repository;
using Animals.ViewModels;

namespace Animals.Controllers
{
    //[Authorize]
    public class OwnersController : Controller
    {
        private AnimalsEntities _context;

        private readonly IRepository<Owner> _ownerRepository;
        private readonly IRepository<Pet> _petRepository;
        private readonly IRepository<Doctor> _doctorRepository;

        public OwnersController()
        {
            _context = new AnimalsEntities();

            _ownerRepository = new SQLRepository<Owner>(_context);
            _petRepository = new SQLRepository<Pet>(_context);
            _doctorRepository = new SQLRepository<Doctor>(_context);
        }

        private string BuildQuery(System.Collections.Specialized.NameValueCollection query)
        {
            var filtersCount = int.Parse(query.GetValues("filterscount")[0]);
            var queryString = @"SELECT * FROM Owners ";
            var tmpDataField = "";
            var tmpFilterOperator = "";
            var where = "";
            if (filtersCount > 0)
            {
                where = " WHERE (";
            }
            for (var i = 0; i < filtersCount; i += 1)
            {
                var filterValue = query.GetValues("filtervalue" + i)[0];
                var filterCondition = query.GetValues("filtercondition" + i)[0];
                var filterDataField = query.GetValues("filterdatafield" + i)[0];
                var filterOperator = query.GetValues("filteroperator" + i)[0];
                if (tmpDataField == "")
                {
                    tmpDataField = filterDataField;
                }
                else if (tmpDataField != filterDataField)
                {
                    where += ") AND (";
                }
                else if (tmpDataField == filterDataField)
                {
                    if (tmpFilterOperator == "")
                    {
                        where += " AND ";
                    }
                    else
                    {
                        where += " OR ";
                    }
                }
                // build the "WHERE" clause depending on the filter's condition, value and datafield.
                where += this.GetFilterCondition(filterCondition, filterDataField, filterValue);
                if (i == filtersCount - 1)
                {
                    where += ")";
                }
                tmpFilterOperator = filterOperator;
                tmpDataField = filterDataField;
            }
            queryString += where;
            return queryString;
        }

        private string GetFilterCondition(string filterCondition, string filterDataField, string filterValue)
        {
            switch (filterCondition)
            {
                case "NOT_EMPTY":
                case "NOT_NULL":
                    return " " + filterDataField + " NOT LIKE '" + "" + "'";
                case "EMPTY":
                case "NULL":
                    return " " + filterDataField + " LIKE '" + "" + "'";
                case "CONTAINS_CASE_SENSITIVE":
                    return " " + filterDataField + " LIKE N'%" + filterValue + "%'" + " COLLATE SQL_Latin1_General_CP1_CS_AS";
                case "CONTAINS":
                    return " " + filterDataField + " LIKE N'%" + filterValue + "%'";
                case "DOES_NOT_CONTAIN_CASE_SENSITIVE":
                    return " " + filterDataField + " NOT LIKE N'%" + filterValue + "%'" + " COLLATE SQL_Latin1_General_CP1_CS_AS"; ;
                case "DOES_NOT_CONTAIN":
                    return " " + filterDataField + " NOT LIKE N'%" + filterValue + "%'";
                case "EQUAL_CASE_SENSITIVE":
                    return " " + filterDataField + " = N'" + filterValue + "'" + " COLLATE SQL_Latin1_General_CP1_CS_AS"; ;
                case "EQUAL":
                    return " " + filterDataField + " = N'" + filterValue + "'";
                case "NOT_EQUAL_CASE_SENSITIVE":
                    return " BINARY " + filterDataField + " <> '" + filterValue + "'";
                case "NOT_EQUAL":
                    return " " + filterDataField + " <> '" + filterValue + "'";
                case "GREATER_THAN":
                    return " " + filterDataField + " > '" + filterValue + "'";
                case "LESS_THAN":
                    return " " + filterDataField + " < '" + filterValue + "'";
                case "GREATER_THAN_OR_EQUAL":
                    return " " + filterDataField + " >= '" + filterValue + "'";
                case "LESS_THAN_OR_EQUAL":
                    return " " + filterDataField + " <= '" + filterValue + "'";
                case "STARTS_WITH_CASE_SENSITIVE":
                    return " " + filterDataField + " LIKE N'" + filterValue + "%'" + " COLLATE SQL_Latin1_General_CP1_CS_AS"; ;
                case "STARTS_WITH":
                    return " " + filterDataField + " LIKE N'" + filterValue + "%'";
                case "ENDS_WITH_CASE_SENSITIVE":
                    return " " + filterDataField + " LIKE N'%" + filterValue + "'" + " COLLATE SQL_Latin1_General_CP1_CS_AS"; ;
                case "ENDS_WITH":
                    return " " + filterDataField + " LIKE N'%" + filterValue + "'";
            }
            return "";
        }

        private IEnumerable<Owner> GetOwnersFromQuery(DbRawSqlQuery<Owner> dbResult)
        {
            var orders = from owner in dbResult
                         select new Owner
                         {
                             Id = owner.Id,
                             Name = owner.Name,
                             Number = owner.Number,
                             Patronymic = owner.Patronymic,
                             Phone = owner.Phone,
                             Sername = owner.Sername,
                             Adress = owner.Adress,
                             Date = owner.Date,
                             Email = owner.Email
                         };
            return orders;
        }

        public JsonResult GetOwners()
        {
            IEnumerable<Owner> owners = null;

            var query = Request.QueryString;
            
            if (query.Get("filterscount")=="0")
            {
                var dbResult = _context.Database.SqlQuery<Owner>("select * from Owners WHERE DATEDIFF( d, Date, GETDATE() ) < 30");
                owners = GetOwnersFromQuery(dbResult);
            }
            else
            {
                var dbResult = _context.Database.SqlQuery<Owner>(this.BuildQuery(query));
                owners = GetOwnersFromQuery(dbResult);
            }

            return Json(owners, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Details(Guid? id)
        {
            Owner owner = _ownerRepository.Find(id.ToGuid());

            var petsNames = (from pet in owner.Pets
                          select new
                          {
                              pet.Nickname,
                              pet.Id,
                              pet.PType,
                              pet.Species,
                              pet.BirthDate
                          });

            return Json(petsNames, JsonRequestBehavior.AllowGet);
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

            using (TransactionScope tr = new TransactionScope())
            {
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

                    if (pet.BirthDate > DateTime.Now)
                        return View(SomethingWentWrong("Дата рождения позже текущей"));

                    _petRepository.Add(pet);
                    _petRepository.SaveAll();

                    tr.Complete();

                    return RedirectToAction("Index", "Owners");
                }

            }

            return View(SomethingWentWrong("Доктор или тим животного не выбран"));
        }

        private ClientsWithPetsVM SomethingWentWrong(string message)
        {
            Populater populater = new Populater();

            ClientsWithPetsVM clientPetsVm = new ClientsWithPetsVM();
            clientPetsVm.PetTypes = populater.PopulatePetTypesList();
            clientPetsVm.ListDoctors = populater.PopulateDoctorsList(_doctorRepository.FindAll().ToList());
            clientPetsVm.Message = message;

            return clientPetsVm;
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
