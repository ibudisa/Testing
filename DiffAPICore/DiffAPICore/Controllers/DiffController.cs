using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DAL.Repositorys;
using DAL.Data;
using DiffAPICore.Models;
using System.Data;
using System.Text;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DiffAPICore.Controllers
{

    public class DiffController : ControllerBase
    {
        private readonly IDataRepository _drepository;

        public DiffController(IDataRepository drepository)
        {
            _drepository = drepository;
        }
        [Route("v1/diff")]
        [HttpGet]
        public ActionResult Get()
        {
            return Ok();
        }
        // get differences by id returning ActionResult with differences
        [Route("v1/diff/{ID}")]
        [HttpGet]
        public ActionResult<DiffInfo> GetByID(int ID)
        {   
            //var diffdata=new DiffInfo();
            var data = _drepository.GetByID(ID);
            if (data.Equals(new DiffInfo()))
                return NotFound();
            else return Ok(data);
        }

        // add leftdata to database returning appropriate response
        [Route("v1/diff/{ID}/left")]
        [HttpPost]
        public ActionResult PostLeft([FromRoute] int ID, [FromBody] DataModel data)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                else
                {
                    string val = data.Data;
                    string decodedval = Encoding.UTF8.GetString(Convert.FromBase64String(val));
                    bool added= _drepository.AddLeft(ID, val);
                    if (added)
                    return StatusCode(201);
                    else
                        return BadRequest();
                }
            }
            catch (Exception ec)
            {
                return StatusCode(500);
            }
        }
        // update leftdata to database returning appropriate response
        [Route("v1/diff/{ID}/left")]
        [HttpPut]
        public ActionResult PutLeft([FromRoute] int ID, [FromBody] DataModel data)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                string val = data.Data;
                string decodedval = Encoding.UTF8.GetString(Convert.FromBase64String(val));
                bool updated = _drepository.UpdateLeft(ID, val);
                if (updated)
                    return StatusCode(201);
                else
                    return NotFound();
            }
            catch (Exception ec)
            {
                return StatusCode(500);
            }
        }

        // add rightdata to database returning appropriate response
        [Route("v1/diff/{ID}/right")]
        [HttpPost]
        public ActionResult PostRight([FromRoute] int ID, [FromBody] DataModel data)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                else
                {
                    string val = data.Data;
                    string decodedval = Encoding.UTF8.GetString(Convert.FromBase64String(val));
                    bool added= _drepository.AddRight(ID, val);
                    if (added)
                        return StatusCode(201);
                    else
                        return BadRequest();
                   
                }
            }
            catch (Exception ec)
            {
                return StatusCode(500);
            }
        }

        // update rightdata to database returning appropriate response
        [Route("v1/diff/{ID}/right")]
        [HttpPut]
        public ActionResult PutRight([FromRoute] int ID, [FromBody] DataModel data)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                string val = data.Data;
                string decodedval = Encoding.UTF8.GetString(Convert.FromBase64String(val));
                bool updated = _drepository.UpdateRight(ID, val);
                if (updated)
                    return StatusCode(201);
                else
                    return NotFound();
            }
            catch (Exception ec)
            {
                return StatusCode(500);
            }
        }

        // delete data from LeftData and RightData tables
        [Route("v1/diff")]
        [HttpDelete]
        public ActionResult Delete()
        {
            bool b = false;
            try
            {
                b= _drepository.EmptyTables();
                if (b)
                    return Ok();
                else
                    return StatusCode(500);
            }
            catch (Exception ec)
            {
                return StatusCode(500);
            }

        }
    }
}
