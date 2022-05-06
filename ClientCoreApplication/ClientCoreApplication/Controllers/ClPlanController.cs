using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DAL;
using DAL.Models;
using DAL.Repositorys;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ClientCoreApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClPlanController : ControllerBase
    {

        private readonly ICleaningPlanRepository _cplanrepository;

        public ClPlanController(ICleaningPlanRepository cplanrepository)
        {
            _cplanrepository = cplanrepository;
        }
        // GET: api/<controller>
        [HttpGet]
        public string Get()
        {
            return "Application started";
        }

       
        [HttpGet("{customerId:int}")]
        public async Task<ActionResult<IEnumerable<CleaningPlan>>> GetByCustomerId(int customerid)
        {
            try
            {
                var list= (await _cplanrepository.GetCleaningPlansByCustomerId(customerid)).ToList();
                return Ok(list);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        // GET api/<controller>/5
        
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<CleaningPlan>> GetById(Guid id)
        {
            try
            {
                var data = await _cplanrepository.GetCleaningPlanById(id);

                if (data == null) return NotFound();

                return Ok(data);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        // POST api/<controller>

        [HttpPost]
        public async Task<ActionResult<CleaningPlan>> Post([FromBody] Customer customer)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (customer == null)
                    return BadRequest();
                   
                var createdplan = await _cplanrepository.AddCleaningPlan(customer);

                return Ok(createdplan);
                
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new cleaningplan record");
            }
            }

        // PUT api/<controller>/5
        [HttpPut("{id:Guid}")]
        public async Task<ActionResult<CleaningPlan>> Put(Guid id, [FromBody]Customer customer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var data = _cplanrepository.GetCleaningPlanById(id);
                if (customer == null || data==null)
                    return BadRequest();

                var updatedplan = await _cplanrepository.UpdateCleaningPlan(customer,id);

                return Ok(updatedplan);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating cleaningplan record");
            }
        }
    

        // DELETE api/<controller>/5
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult<CleaningPlan>> Delete(Guid id)
        {
         try
         {
            var data= await _cplanrepository.GetCleaningPlanById(id);
            if (data == null)
            {
                return NotFound($"CleaningPlan with Id = {id} not found");
            }
            var plan = await _cplanrepository.DeleteCleaningPlan(id);
            return Ok(plan);
          }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }

    }
    
}
