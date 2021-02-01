using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PromoWebService.Models
{
    public class DettPromo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 Id { get; set; }

        public string IdPromo { get; set; }

        [Required]
        public Int16? Riga { get; set; }

        [Required]
        public string CodArt { get; set; }

        public string CodFid { get; set; }

        [Required]
        public DateTime? Inizio { get; set; }

        [Required]
        public DateTime? Fine { get; set; }

        public Int16? IdTipoPromo { get; set; }

        [Required]
        public string Oggetto { get; set; }

        public string IsFid { get; set; }

        //proprietà di collegamento classi models
        public virtual Promo promo { get; set; }
        public virtual TipoPromo tipoPromo { get; set; }

    }
}