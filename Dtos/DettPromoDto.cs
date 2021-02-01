using System;

namespace PromoWebService.Dtos
{
    public class DettPromoDto
    {
        public Int64 Id { get; set; }
        public Int16? Riga { get; set; }
        public string CodArt { get; set; }
        public string Descrizione { get; set; }
        public decimal? Prezzo { get; set; }
        public string CodFid { get; set; }
        public DateTime? Inizio { get; set; }
        public DateTime? Fine { get; set; }
        public string TipoPromo { get; set; }
        public string Oggetto { get; set; }
        public string IsFid { get; set; }

    }
}