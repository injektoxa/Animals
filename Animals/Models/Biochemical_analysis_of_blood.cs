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
    
    public partial class Biochemical_analysis_of_blood
    {
        public System.Guid Id { get; set; }
        public Nullable<System.Guid> PetId { get; set; }
        public string Path { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<decimal> Mochevina { get; set; }
        public Nullable<decimal> Kreatinin { get; set; }
        public Nullable<decimal> Holesterin { get; set; }
        public Nullable<decimal> Glyukoza { get; set; }
        public Nullable<decimal> ShelochnayaFosftaza { get; set; }
        public Nullable<decimal> Alat { get; set; }
        public Nullable<decimal> Asat { get; set; }
        public Nullable<decimal> CommonProtein { get; set; }
        public Nullable<decimal> Albumin { get; set; }
        public Nullable<decimal> Globulin { get; set; }
        public Nullable<decimal> Na { get; set; }
        public Nullable<decimal> K { get; set; }
        public Nullable<decimal> Ca { get; set; }
        public Nullable<decimal> P { get; set; }
        public Nullable<decimal> Clhoriodu { get; set; }
        public Nullable<decimal> CommonBilubrin { get; set; }
        public Nullable<decimal> Solidbilirubin { get; set; }
        public Nullable<decimal> lDG { get; set; }
        public Nullable<decimal> KreatinKinaza { get; set; }
        public Nullable<decimal> GGT { get; set; }
        public Nullable<decimal> Amilaza { get; set; }
        public Nullable<decimal> Lipaza { get; set; }
    
        public virtual Pet Pet { get; set; }
    }
}
