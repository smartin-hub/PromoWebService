using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using PromoWebService.Models;

namespace PromoWebService.Services
{
    public interface IPromoRepository
    {
        Task<Promo> SelPromoById(string IdPromo);
        Promo SelPromoById2(string IdPromo);
        Promo SelPromoByCode(int Anno, string Codice);
        decimal SelPrzPromo(string CodArt);
        Task<decimal> SelPrzPromoSql(string CodArt);
        Task<ICollection<DettPromo>> SelPromoActive();
        Task<ICollection<VwDettPromo>> SelPromoActive1();

        Task<bool> PromoExists(string IdPromo);
        bool InsPromo(Promo promozione);
        bool UpdPromo(Promo promozione);
        bool DelPromo(Promo promozione);
    }
}