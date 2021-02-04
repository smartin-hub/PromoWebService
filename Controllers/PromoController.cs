using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ArticoliWebService.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PromoWebService.Dtos;
using PromoWebService.Models;
using PromoWebService.Services;
using PromoWebService.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace PromoWebService.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/promo")]
    [Authorize(Roles = "ADMIN, USER")]
    public class PromoController : Controller
    {
        private readonly IPromoRepository promoRepository;

        private readonly ArtWebApi artWebApi;

        public PromoController(IPromoRepository promoRepository, IOptions<ArtWebApi> priceWebApi)
        {
            this.promoRepository = promoRepository;
            this.artWebApi = priceWebApi.Value;
        }

        private async Task<ArticoliDto> getArtAsync(string CodArt, string IdList, string Token)
        {
            ArticoliDto articolo = null;

            using (var client = new HttpClient())
            {
                Token = (Token != null) ? Token.Replace("Bearer ","") : "";

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                string Uri = this.artWebApi.EndPoint + CodArt + "/" + IdList;
                
                var result = await client.GetAsync(Uri);
                var response = await result.Content.ReadAsStringAsync();
                articolo = JsonConvert.DeserializeObject<ArticoliDto>(response);
            }
        
            return articolo;
        }

        private async Task<PromoDto> CreatePromoDTO(Promo promo, string accessToken)
        {
            var dettPromoDto = new List<DettPromoDto>();
            
            foreach(var dettpromo in promo.dettPromo)
            {
                ArticoliDto articoloDTO = await getArtAsync(dettpromo.CodArt, this.artWebApi.Listino, accessToken);

                dettPromoDto.Add(new DettPromoDto
                {
                    Id = dettpromo.Id,
                    Riga = dettpromo.Riga,
                    CodArt = dettpromo.CodArt,
                    Descrizione = (articoloDTO == null) ? "" : articoloDTO.Descrizione.Trim(),
                    Prezzo = articoloDTO.Prezzo,
                    CodFid = dettpromo.CodFid,
                    Inizio = dettpromo.Inizio,
                    Fine = dettpromo.Fine,
                    TipoPromo = (dettpromo.tipoPromo.Descrizione == null) ? "" : dettpromo.tipoPromo.Descrizione, 
                    Oggetto = dettpromo.Oggetto,
                    IsFid = dettpromo.IsFid

                });
            }

            var promoDto = new PromoDto
            {
                IdPromo = promo.IdPromo,
                Descrizione = promo.Descrizione.Trim(),
                Codice = promo.Codice.Trim(),
                Anno = promo.Anno,
                DettPromo = dettPromoDto.OrderBy(a => a.Riga).ToList()
            };

            return promoDto;
        }

        [HttpGet("id/{IdPromo}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(PromoDto))]
        public async Task<ActionResult<PromoDto>> GetPromoById(string IdPromo)
        {
            string accessToken = Request.Headers["Authorization"];

            //IdList = (IdList == null) ? this.priceWebApi.Listino : IdList;

            if (!await this.promoRepository.PromoExists(IdPromo))
            {
                return NotFound(new InfoMsg(DateTime.Today, $"Non è stata trovata la Promo con Id {IdPromo}"));
            }

            var promo = await this.promoRepository.SelPromoById(IdPromo);

            PromoDto retVal = await CreatePromoDTO(promo, accessToken);

            return Ok(retVal);
        }
        
        [HttpGet("prezzo/{CodArt}")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        public ActionResult<decimal> GetPrezzoPromo(string CodArt)
        {
            return Ok(this.promoRepository.SelPrzPromo(CodArt));
        }

        [HttpGet("prezzo2/{CodArt}")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        public async Task<ActionResult<decimal>> GetPrezzoPromoSql(string CodArt)
        {
            return Ok(await this.promoRepository.SelPrzPromoSql(CodArt));
        }


        [HttpGet("active")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(PromoDto))]
        public async Task<IActionResult> GetPromoActive()
        {
            string accessToken = Request.Headers["Authorization"];

            //IdList = (IdList == null) ? this.priceWebApi.Listino : IdList;

            var promo = await this.promoRepository.SelPromoActive();

            if (promo.Count == 0)
            {
                return NotFound(new InfoMsg(DateTime.Today, $"Non è stata trovata alcuna Promo attiva!"));
            }

            var artPromoDto = new List<ArtPromoDto>();
            
            foreach(var dettpromo in promo)
            {
                ArticoliDto articoloDTO = await getArtAsync(dettpromo.CodArt, this.artWebApi.Listino, accessToken);

                if (articoloDTO != null)
                {
                    artPromoDto.Add(new ArtPromoDto
                    {
                        
                        Id = dettpromo.Id,
                        CodArt = dettpromo.CodArt,
                        Descrizione = (articoloDTO == null) ? "" : articoloDTO.Descrizione.Trim(),
                        Prezzo = articoloDTO.Prezzo,
                        Fine = dettpromo.Fine,
                        TipoPromo = dettpromo.IdTipoPromo,
                        Oggetto = dettpromo.Oggetto,
                        IsFid = dettpromo.IsFid
                    });
                }
                else
                    return BadRequest(new ErrMsg("Impossibile accedere al servizio Articoli", "400"));
                
            }
            

            return Ok(artPromoDto);
        }

        [HttpGet("active1")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(PromoDto))]
        public async Task<IActionResult> GetPromoActive1()
        {
            string accessToken = Request.Headers["Authorization"];

            //IdList = (IdList == null) ? this.priceWebApi.Listino : IdList;

            var promo = await this.promoRepository.SelPromoActive1();

            if (promo.Count == 0)
            {
                return NotFound(new InfoMsg(DateTime.Today, $"Non è stata trovata alcuna Promo attiva!"));
            }

            var artPromoDto = new List<ArtPromoDto>();
            
            foreach(var dettpromo in promo)
            {
                ArticoliDto articoloDTO = await getArtAsync(dettpromo.CodArt, this.artWebApi.Listino, accessToken);

                if (articoloDTO != null)
                {
                    artPromoDto.Add(new ArtPromoDto
                    {
                        //se si usa la stored non si può usare perchè la stored non tira su l'id
                        //Id = dettpromo.Id,
                        CodArt = dettpromo.CodArt,
                        Descrizione = (articoloDTO == null) ? "" : articoloDTO.Descrizione.Trim(),
                        Prezzo = articoloDTO.Prezzo,
                        Fine = dettpromo.Fine,
                        TipoPromo = dettpromo.TipoPromo,
                        Oggetto = dettpromo.Oggetto,
                        IsFid = dettpromo.IsFid
                    });
                }
                else
                    return BadRequest(new ErrMsg("Impossibile accedere al servizio Articoli", "400"));
                
            }
            

            return Ok(artPromoDto);
        }

        [HttpPost("inserisci")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "ADMIN")]
        public ActionResult<InfoMsg> SavePromo([FromBody] Promo promo)
        {
            if (promo == null)
            {
                return BadRequest(ModelState);
            }

            string guid = Guid.NewGuid().ToString();

            promo.IdPromo = guid;

            var dettPromo = promo.dettPromo.ToList();
            dettPromo.ForEach(x => x.IdPromo = guid);

            promo.dettPromo = dettPromo;

            var depRifPromo = promo.depRifPromo.ToList();
            depRifPromo.ForEach(x => x.IdPromo = guid);

            promo.depRifPromo = depRifPromo;

            //Verifichiamo che i dati siano corretti
            if (!ModelState.IsValid)
            {
                string ErrVal = "";

                string errore = (this.HttpContext == null) ? "400" : this.HttpContext.Response.StatusCode.ToString();

                foreach (var modelState in ModelState.Values) 
                {
                    foreach (var modelError in modelState.Errors) 
                    {
                        ErrVal += modelError.ErrorMessage + " - "; 
                    }
                }

                return BadRequest(new ErrMsg(ErrVal,errore));
            }

            //Contolliamo se l'articolo è presente
            var isPresent = promoRepository.SelPromoByCode(promo.Anno, promo.Codice);

            if (isPresent != null)
            {
                return StatusCode(422, new ErrMsg($"Il codice {promo.Codice} dell'anno {promo.Anno} è attribuito alla promozione {isPresent.IdPromo} scegliere un altro codice","422"));
            }

            //verifichiamo che i dati siano stati regolarmente inseriti nel database
            if (!promoRepository.InsPromo(promo))
            {
                ModelState.AddModelError("", $"Ci sono stati problemi nell'inserimento della promozione {promo.IdPromo} ");
                return StatusCode(500, ModelState);
            }

            return Ok(new InfoMsg(DateTime.Today, $"Inserimento promozione {promo.IdPromo} eseguita con successo!"));
            
        }

        [HttpPut("modifica")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "ADMIN")]
        public ActionResult<InfoMsg> UpdatePromo([FromBody] Promo promo)
        {
            if (promo == null)
            {
                return BadRequest(ModelState);
            }

            //Verifichiamo che i dati siano corretti
            if (!ModelState.IsValid)
            {
                string ErrVal = "";

                string errore = (this.HttpContext == null) ? "400" : this.HttpContext.Response.StatusCode.ToString();

                foreach (var modelState in ModelState.Values) 
                {
                    foreach (var modelError in modelState.Errors) 
                    {
                        ErrVal += modelError.ErrorMessage + " - "; 
                    }
                }

                return BadRequest(new ErrMsg(ErrVal,errore));
            }

            //Contolliamo se l'articolo è presente
            var isPresent = promoRepository.SelPromoById2(promo.IdPromo);

            if (isPresent == null)
            {
                return StatusCode(422, new ErrMsg($"Promozione {promo.IdPromo} NON presente in anagrafica! " +
                    "Impossibile utilizzare il metodo PUT!","422"));
            }

            //verifichiamo che i dati siano stati regolarmente inseriti nel database
            if (!promoRepository.UpdPromo(promo))
            {
                ModelState.AddModelError("", $"Ci sono stati problemi nella modifica della promozione {promo.IdPromo} ");
                return StatusCode(500, ModelState);
            }

            return Ok(new InfoMsg(DateTime.Today, $"Modifica promozione {promo.IdPromo} eseguita con successo!"));
            
        }

        [HttpDelete("elimina/{idPromo}")]
        [ProducesResponseType(201, Type = typeof(InfoMsg))]
        [ProducesResponseType(400, Type = typeof(ErrMsg))]
        [ProducesResponseType(422, Type = typeof(ErrMsg))]
        [ProducesResponseType(500, Type = typeof(ErrMsg))]
        [Authorize(Roles = "ADMIN")]
        public ActionResult<InfoMsg> DeletePromo(string idPromo)
        {
            if (idPromo == "")
            {
                return BadRequest(new ErrMsg($"E' necessario inserire il codice della promozione da eliminare!",
                    this.HttpContext.Response.StatusCode.ToString()));
            }

            //Contolliamo se l'articolo è presente (Usare il metodo senza Traking)
            var promo = promoRepository.SelPromoById2(idPromo);

            if (promo == null)
            {
                return StatusCode(422, new ErrMsg($"Promozione {idPromo} NON presente in anagrafica! Impossibile Eliminare!",
                    "422"));
            }

             //verifichiamo che i dati siano stati regolarmente eliminati dal database
            if (!promoRepository.DelPromo(promo))
            {
                return StatusCode(500, new ErrMsg($"Ci sono stati problemi nella eliminazione della promozione {idPromo}.",
                    "500"));
            }

            return Ok(new InfoMsg(DateTime.Today, $"Eliminazione promozione {idPromo} eseguita con successo!"));
        }

    }
}