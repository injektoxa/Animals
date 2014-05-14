using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Animals.Models
{

    [MetadataType(typeof(PetMetaData))]
    public partial class Pet
    {

    }

    public class PetMetaData
    {
        [Required(ErrorMessage = "Заполните кличку")]
        public string Nickname { get; set; }
        
        public string PType { get; set; }

        [Required(ErrorMessage = "Заполните породу")]
        public string Species { get; set; }

        [Required(ErrorMessage = "Заполните пол")]
        public string Gender { get; set; }

        [Required]
        public string DoctorId { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public string BirthDate { get; set; }
    }
}