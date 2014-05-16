using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Animals.Models
{
    public class Populater
    {
        public List<string> PopulatePetTypesList()
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

        public List<DoctorForDDl> PopulateDoctorsList(AnimalsEntities db)
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
    }
}