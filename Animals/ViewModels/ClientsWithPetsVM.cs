using Animals.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Animals.ViewModels
{
    public class ClientsWithPetsVM
    {
        public Pet Pet { set; get; }
        public Owner Owner { set; get; }
        public IEnumerable PetTypes { set; get; }
        public IEnumerable DoctorId { set; get; }
    }
}