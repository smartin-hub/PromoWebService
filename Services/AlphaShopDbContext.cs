using Microsoft.EntityFrameworkCore;
using PromoWebService.Models;

namespace ArticoliWebService.Services
{
    public class AlphaShopDbContext : DbContext
    {
        public AlphaShopDbContext(DbContextOptions<AlphaShopDbContext> options)
            : base(options)
        {
            
        }

        public virtual DbSet<Promo> Promo { get; set; }
        public virtual DbSet<DettPromo> DettPromo {get; set; }
        public virtual DbSet<DepRifPromo> DepRifPromo { get; set; }
        public virtual DbSet<TipoPromo> TipoPromo { get; set; }
        

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            
            //Relazione one to many (uno a molti) fra Promo e DettPromo
            modelBuilder.Entity<DettPromo>()
                .HasOne<Promo>(s => s.promo) //ad una promozione...
                .WithMany(g => g.dettPromo) //corrispondono molti dettaglio promozione
                .HasForeignKey(s => s.IdPromo); //la chiave esterna dell'entity dettPromo

              //Relazione one to many fra Promo e DepRifPromo 
            modelBuilder.Entity<DepRifPromo>()
                .HasOne<Promo>(s => s.promo) //ad una promozione...
                .WithMany(g => g.depRifPromo) // corrispondono molti depositi di riferimento
                .HasForeignKey(s => s.IdPromo);

            //Relazione one to many fra TipoPromo e DettPromo
            modelBuilder.Entity<DettPromo>()
                .HasOne<TipoPromo>(s => s.tipoPromo)
                .WithMany(g => g.dettPromo)
                .HasForeignKey(s => s.IdTipoPromo);

        }

        
    }
}