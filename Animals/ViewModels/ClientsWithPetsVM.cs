using Animals.Models;
using System.Collections;

namespace Animals.ViewModels
{
    public class ClientsWithPetsVM
    {
        public Pet Pet { set; get; }
       
        public Owner Owner { set; get; }
        
        public IEnumerable PetTypes { set; get; }
        
        public IEnumerable ListDoctors { set; get; }

        public string Message { set; get; }
    }
}