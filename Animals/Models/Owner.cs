//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Animals.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Owner
    {
        public Owner()
        {
            this.Pets = new HashSet<Pet>();
        }
    
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public string Sername { get; set; }
        public string Patronymic { get; set; }
        public string Adress { get; set; }
        public string Phone { get; set; }
        public System.DateTime Date { get; set; }
        public int Number { get; set; }
        public string Email { get; set; }
    
        public virtual ICollection<Pet> Pets { get; set; }
    }
}
