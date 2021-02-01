using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArticoliWebService.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PromoWebService.Models;

namespace PromoWebService.Services
{
    public class PromoRepository : IPromoRepository
    {
        AlphaShopDbContext alphaShopDbContext;

        public PromoRepository(AlphaShopDbContext alphaShopDbContext)
        {
            this.alphaShopDbContext =  alphaShopDbContext;
        }

        public bool InsPromo(Promo promozione)
        {
            this.alphaShopDbContext.Add(promozione);
            return Salva();
        }

        public bool UpdPromo(Promo promozione)
        {
            this.alphaShopDbContext.Update(promozione);
            return Salva();
        }

        public bool DelPromo(Promo promozione)
        {
            this.alphaShopDbContext.Remove(promozione);
            return Salva();
        }

        public bool Salva()
        {
            var saved = this.alphaShopDbContext.SaveChanges();
            return saved >= 0 ? true : false; 
        }

        public async Task<bool> PromoExists(string IdPromo)  => await 
                this.alphaShopDbContext
                .Promo
                .AnyAsync(c => c.IdPromo.Trim() == IdPromo);

        public decimal SelPrzPromo(string CodArt)
        {
            decimal retVal = 0;

            var collection = this.alphaShopDbContext.DettPromo as IQueryable<DettPromo>;

            collection = collection.Where(
                a => a.CodArt.Equals(CodArt)  //Filtro per Codice
                && a.Inizio <= DateTime.Today // Da Data
                && a.Fine >= DateTime.Today // A Data
                && a.IdTipoPromo == 1 //Id Promo 1 (Taglio Prezzo)
            ).OrderBy(a => a.Oggetto); // Riordino dal più basso

            List<DettPromo> dettPromo = collection.ToList();

            if (dettPromo.Count > 0)
            {
                try
                {
                    var items = dettPromo.Take(1);
                    foreach(DettPromo item in items)
                    {
                        retVal = decimal.Parse(item.Oggetto.Replace(".",","));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            
            }    
        
            return retVal;
        }

        public async Task<decimal> SelPrzPromoSql(string CodArt)
        {
            decimal retVal = 0;

            string Sql = "EXEC Sp_SelPrezzoPromo @CodArt";

            var parCode = new SqlParameter("@CodArt ", CodArt);

            var collection = await this.alphaShopDbContext.DettPromo
                .FromSqlRaw(Sql, parCode)
                .ToListAsync();

            List<DettPromo> dettPromo = collection.ToList();

            if (dettPromo.Count > 0)
            {
                try
                {
                    var items = dettPromo.Take(1);
                    foreach(DettPromo item in items)
                    {
                        retVal = decimal.Parse(item.Oggetto.Replace(".",","));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            
            }    
        
            return retVal;

        }

        public async Task<Promo> SelPromoById(string IdPromo)
        {
            
            return await this.alphaShopDbContext
                .Promo
                .Include(a => a.dettPromo)
                    .ThenInclude(b => b.tipoPromo)
                .Where(a => a.IdPromo == IdPromo)
                .FirstOrDefaultAsync();
        }

        public Promo SelPromoById2(string IdPromo)
        {
            return this.alphaShopDbContext
                .Promo
                .AsNoTracking()
                .Where(a => a.IdPromo == IdPromo)
                .FirstOrDefault();
        }

        public Promo SelPromoByCode(int Anno, string Codice)
        {
            return this.alphaShopDbContext
                .Promo
                .AsNoTracking()
                .Where(a => a.Anno == Anno)
                .Where(a => a.Codice == Codice)
                .FirstOrDefault();
        }

        public async Task<ICollection<DettPromo>> SelPromoActive()
        {
            //string Sql = $"SELECT * FROM DETTPROMO WHERE GETDATE() BETWEEN INIZIO AND FINE;";

            return await this.alphaShopDbContext
            .DettPromo
            .Where(a => a.Inizio <= DateTime.Today // Da Data
                && a.Fine >= DateTime.Today) // A Data
            //.FromSqlRaw(Sql)
            .ToListAsync();
            
        }
    }
}