using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PromoWebService.Models
{
    public class Promo
    {
        [Key]
        //[MinLength(5, ErrorMessage = "Inserisci l'Id della Promozione" )]
        public string IdPromo { get; set; }

        [Required]
        public Int16 Anno { get; set; }

        [Required]
        [MaxLength(10, ErrorMessage = "Il codice può avere un massimo di 10 caratteri" )]
        public string Codice { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "La Descrizione può avere un massimo di 50 caratteri" )]
        public string Descrizione { get; set; }

        //proprietà di collegamento classi models
        public virtual ICollection<DettPromo> dettPromo { get; set; }
        public virtual ICollection<DepRifPromo> depRifPromo { get; set; }


    }
}