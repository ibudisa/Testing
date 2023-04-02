using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DAL.Entiteti;
using DAL.Servisi;
using VanadoWebAPI.Models;
using VanadoWebAPI.Helpers;

namespace VanadoWebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class KvaroviController : ControllerBase
    {
        private readonly IKvaroviServis _kvaroviServis;

        public KvaroviController(IKvaroviServis kvaroviServis)
        {
            _kvaroviServis = kvaroviServis;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _kvaroviServis.DohvatiKvarove();

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetKvar(int id)
        {
            var result = await _kvaroviServis.DohvatiKvar(id);

            if (result == null) return NotFound();
            else
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddKvar([FromBody] KvaroviModel kvar)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                else
                {
                    bool sadrzi = await _kvaroviServis.ProvjeriKvar(kvar.Idstroja);
                    if (!sadrzi)
                    {
                        Kvarovi kv = new Kvarovi();
                        kv = MapData.MapirajKvarovi(kvar, 0);
                        var result = await _kvaroviServis.DodajKvar(kv);

                        return Ok(result);
                    }
                    else return BadRequest();
                }
            }
            catch (Exception ec)
            {
                return StatusCode(500);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateKvar([FromBody] KvaroviModel kvar, int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                else
                {
                    Kvarovi kv = new Kvarovi();
                    kv = MapData.MapirajKvarovi(kvar, id);
                    var result = await _kvaroviServis.PromijeniKvar(kv);

                    return Ok(result);
                }
            }
            catch (Exception ec)
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteKvar(int id)
        {
            var kvar = await _kvaroviServis.DohvatiKvar(id);

            if (kvar == null) return NotFound();
            else
            {
                var result = await _kvaroviServis.ObrisiKvar(id);

                return Ok(result);
            }
        }

        [HttpPut("{id:int}/{status:bool}")]
        public async Task<IActionResult> UpdateStatus(int id,bool status)
        {
            //bool st = false;
            //if(status.Equals("True"))
            //    st= true;   

            var result = await _kvaroviServis.PromijeniStatusKvara(status,id);

            return Ok(result);
  
        }

        [HttpGet("{odmak:int}/{broj:int}")]
        public async Task<IActionResult> GetOdredjeneKvarove(int odmak,int broj)
        {
            try
            {
                var result = await _kvaroviServis.DohvatiOdredjeneKvarove(odmak, broj);

                return Ok(result);
            }
            catch(Exception ec)
            {
                return StatusCode(500);
            }
        }
    }
}
