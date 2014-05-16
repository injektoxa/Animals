using Animals.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Animals.ViewModels
{
    public class PetsVM
    {
        public Pet Pet { set; get; }

        public IEnumerable PetTypes { set; get; }
        
        public IEnumerable ListDoctors { set; get; }

        public string Message { set; get; }
    }
}