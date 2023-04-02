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
    public class StrojeviController : ControllerBase
    {
        private readonly IStrojeviServis _strojeviServis;

        public StrojeviController(IStrojeviServis strojeviServis)
        {
            _strojeviServis = strojeviServis;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _strojeviServis.DohvatiStrojeve();

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetStroj(int id)
        {
            var result = await _strojeviServis.DohvatiStroj(id);
            if(result==null) return NotFound();
            else
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddStroj([FromBody] StrojeviModel stroj)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                else
                {
                    bool sadrzi = await _strojeviServis.ProvjeriNaziv(stroj.Naziv);
                    if (!sadrzi)
                    {
                        Strojevi strojevi = new Strojevi();
                        strojevi = MapData.MapirajStrojevi(stroj, 0);
                        var result = await _strojeviServis.DodajStroj(strojevi);

                        return Ok(result);
                    }
                    else return BadRequest();
                }
            }
            catch(Exception ec)
            {
                return StatusCode(500);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateStroj([FromBody] StrojeviModel stroj, int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                else
                {
                    bool sadrzi = await _strojeviServis.ProvjeriNaziv(stroj.Naziv);
                    if (!sadrzi)
                    {
                        Strojevi strojevi = new Strojevi();
                        strojevi = MapData.MapirajStrojevi(stroj, id);
                        var result = await _strojeviServis.PromijeniStroj(strojevi);

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

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteStroj(int id)
        {
            var stroj = await _strojeviServis.DohvatiStroj(id);
            if (stroj == null)
                return BadRequest();
            else
            {
                var result = await _strojeviServis.ObrisiStroj(id);

                return Ok(result);
            }
        }
    }
}
