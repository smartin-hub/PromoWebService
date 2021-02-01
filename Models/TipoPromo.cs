using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PromoWebService.Models
{
    public class TipoPromo
    {
        [Key]
        public Int16 IdTipoPromo { get; set; }
        public string Descrizione { get; set; }

        public virtual ICollection<DettPromo> dettPromo { get; set; }
    }
}