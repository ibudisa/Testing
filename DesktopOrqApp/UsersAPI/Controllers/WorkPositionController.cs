using DAL;
using DAL.Entities;
using DAL.Repositorys;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace UsersAPI.Controllers
{
    [ApiController]
    public class WorkPositionController : ControllerBase
    {

        private readonly IDataRepository _repository;

        public WorkPositionController(IDataRepository dataRepository)
        {
            _repository = dataRepository;
        }

        [Route("[controller]/GetAll")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkPosition>>> Get()
        {
            var list = await _repository.GetAllWorkPositions();
            return Ok(list);
        }
        [Route("UWPositions/Login/{email}/{password}")]
        [HttpGet]
        public async Task<ActionResult<DataPosition>> GetUserRoleData(string email,string password)
        {
            var user = await _repository.GetUserByEmailAndPassword(email, password);
            if (user is null) return NotFound();
            if (user.UserRoleInfo is null) return NotFound();
            //var positonval = _repository.GetInfoByUserId(user.Id);

            return Ok(user);
        }

        [Route("UWPositions/Get")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User_WorkPosition>>> GetUserWorkPositions()
        {
            var list =await _repository.GetAllUserWorkPositions();
            return Ok(list);
        }
        [Route("[controller]/GetById")]
        [HttpGet]
        public async Task<ActionResult<WorkPosition>> GetWorkPositionById(int id)
        {
            var wposition =await _repository.GetWorkPositionById(id);
            if (wposition == null)
            {
                return NotFound();
            }
            return Ok(wposition);
        }
        [Route("[controller]/GetMaxId")]
        [HttpGet]
        public async Task<ActionResult<int>> GetWorkPositionMaxId()
        {
            var maxid = await _repository.GetWorkPositionMaxId();
          
            return Ok(maxid);
        }

        [Route("UWPositions/GetUWMaxId")]
        [HttpGet]
        public async Task<ActionResult<int>> GetUserWorkPositionMaxId()
        {
            var maxid = await _repository.GetUserWorkPositionMaxId();

            return Ok(maxid);
        }

        [Route("[controller]/Add")]
        [HttpPost]
        public async Task<ActionResult<bool>> AddWorkPosition(WorkPosition wposition)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool added =await _repository.AddWorkPosition(wposition);
            return Ok(added);
        }
        [Route("UWPositions/Add")]
        [HttpPost]
        public async Task<ActionResult<bool>> AddUserWorkPosition(User_WorkPosition uwposition)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool added =await _repository.AddUserWorkPosition(uwposition);
            return Ok(added);
        }
        [Route("[controller]/Update")]
        [HttpPut]
        public async Task<ActionResult<bool>> UpdateWorkPosition([FromBody] WorkPosition position)
        {
           
            if (_repository.GetWorkPositionById(position.Id) is null)
            {
                return NotFound();
            }
            bool updated=await _repository.UpdateWorkPosition(position);
            return Ok(updated);
        }
        [Route("[controller]/Delete")]
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteWorkPosition(int id)
        {
            if (_repository.GetWorkPositionById(id) is null)
            {
                return NotFound();
            }
            bool deleted=await _repository.DeleteWorkPosition(id);
            return Ok(deleted);
        }
    }   
}
