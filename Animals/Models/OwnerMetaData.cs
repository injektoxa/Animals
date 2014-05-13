using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Animals.Models
{
    [MetadataType(typeof(OwnerMetaData))]
    public partial class Owner
    {

    }

    public class OwnerMetaData
    {
        [Required(ErrorMessage = "Заполните имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Заполните фамилию")]
        public string Sername { get; set; }

        [Required(ErrorMessage = "Заполните адрес")]
        public string Adress { get; set; }

        [Required(ErrorMessage = "Заполните телефон")]
        public string Phone { get; set; }
    }
}