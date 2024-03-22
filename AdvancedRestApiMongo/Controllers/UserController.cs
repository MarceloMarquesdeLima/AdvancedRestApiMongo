using AdvancedRestApiMongo.DTOs;
using AdvancedRestApiMongo.Interfaces;
using AdvancedRestApiMongo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;


namespace AdvancedRestApiMongo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUser _userService;

        public UserController(IUser userService)
        {
            _userService = userService;
        }

        // GET: api/<UserController>
        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            var result = await _userService.GetAllUsers();
            if (result.IsSuccess)
            {
                return Ok(result.User);
            }
            return NotFound(result.ErrorMessage);
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _userService.GetUserById(id);
            if(result.IsSuccess)
            {
                return Ok(result.User); 
            }
            return NotFound(result.ErrorMessage);
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserDTO user)
        {
            var result = await _userService.AddUser(user);
            if(result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status201Created);
            }
            return BadRequest(result.ErrorMessage);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UserDTO user)
        {
            var result = await _userService.UpdateUser(id, user);
            if(result.IsSuccess )
            {
                return NoContent();
            }
            return BadRequest(result.ErrorMessage);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userService.DeleteUser(id);
            if(result.IsSuccess )
            {
                return NoContent();
            }
            return BadRequest(result.ErrorMessage);
        }
    }
}
