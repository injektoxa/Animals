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
    
    public partial class Anamne
    {
        public System.Guid Id { get; set; }
        public string Text { get; set; }
        public Nullable<System.Guid> PetId { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
    
        public virtual Pet Pet { get; set; }
    }
}
