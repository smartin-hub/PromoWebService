using System;
using System.Collections.Generic;
using PromoWebService.Models;

namespace ArticoliWebService.Dtos
{
    public class ArticoliDto
    {
        public string CodArt { get; set; }
        public string  Descrizione { get; set; }
        public decimal Prezzo { get; set; }
        
    }
}