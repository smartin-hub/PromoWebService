using System;

namespace PromoWebService.Dtos
{
    public class ArtPromoDto
    {
        public Int64 Id { get; set; }
        public string CodArt { get; set; }
        public string Descrizione { get; set; }
        public decimal? Prezzo { get; set; }
        public DateTime? Fine { get; set; }
        public short? TipoPromo { get; set; }
        public string Oggetto { get; set; }
        public string IsFid { get; set; }


    }
}