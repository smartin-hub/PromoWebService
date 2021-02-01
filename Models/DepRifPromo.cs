using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PromoWebService.Models
{
    public class DepRifPromo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 Id { get; set; }
        public string IdPromo { get; set; }
        public int IdDeposito { get; set; }

        public virtual Promo promo { get; set; }
    }
}