using Hasura.GraphQl.Backend.Dtos;
using Hasura.GraphQl.Backend.services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hasura.GraphQl.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly HasuraService _hasuraService;


        public UserController(HasuraService hasuraService)
        {
            _hasuraService = hasuraService;
            
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _hasuraService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpGet]
        public async Task<ActionResult<List<Users>>> GetAllUsers()
        {
            var user = await _hasuraService.GetAllUsersAsync();
            if (user == null) return NotFound();
            return Ok(user);
        }

     



    }
}
