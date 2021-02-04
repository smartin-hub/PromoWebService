using System;

namespace Models
{
    //Mappa una query (stored procedure Sp_SelPromoActive)
    //i campi non sono Case Sensitive
    //i tipi devono essere rispettati
    public class VwDettPromo
    {
        public string CodArt {get; set;}
        public string Descrizione {get; set;}
        public decimal Prezzo {get; set;}
        public DateTime Fine {get; set;}
        public Int16 TipoPromo {get; set;}
        public string Oggetto {get; set;}
        public string IsFid {get; set;}
    }
}