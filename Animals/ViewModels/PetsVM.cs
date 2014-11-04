using Animals.Models;
using System.Collections;

namespace Animals.ViewModels
{
    public class PetsVM
    {
        public Pet Pet { set; get; }

        public IEnumerable PetTypes { set; get; }
        
        public IEnumerable ListDoctors { set; get; }

        public string Message { set; get; }

        public string DoctorName { set; get; }

        public string PetType { set; get; }
    }
}