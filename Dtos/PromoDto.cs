using System.Collections.Generic;

namespace PromoWebService.Dtos
{
    public class PromoDto
    {
        public string IdPromo { get; set; }
        public int Anno { get; set; }
        public string  Codice { get; set; }
        public string Descrizione { get; set; }

        public ICollection<DettPromoDto> DettPromo { get; set; }
    }
}