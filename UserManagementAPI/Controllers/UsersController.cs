using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Data;
using UserManagementAPI.DTOs;
using UserManagementAPI.Models;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserStore _store;

        public UsersController(UserStore store)
        {
            _store = store;
        }

        // GET api/users  - returns all users (public, no auth needed)
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_store.GetAll());
        }

        // GET api/users/{id}  - returns one user or 404
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _store.GetById(id);
            if (user == null)
                return NotFound(new { message = $"No user found with id {id}." });

            return Ok(user);
        }

        // POST api/users  - creates a user, returns 201 with location header
        [HttpPost]
        public IActionResult Create([FromBody] CreateUserDto dto)
        {
            // 400 if any DataAnnotation fails (missing field, bad email, etc.)
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Manual email duplicate check (no DB, so we check the in-memory list)
            if (_store.EmailExists(dto.Email))
                return Conflict(new { message = "A user with this email already exists." });

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName  = dto.LastName,
                Email     = dto.Email,
                Age       = dto.Age
            };

            _store.Add(user);

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        // PUT api/users/{id}  - updates an existing user, returns 204
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = _store.GetById(id);
            if (user == null)
                return NotFound(new { message = $"No user found with id {id}." });

            // Check duplicate email but exclude the current user's own email
            if (_store.EmailExists(dto.Email, excludeId: id))
                return Conflict(new { message = "A user with this email already exists." });

            user.FirstName = dto.FirstName;
            user.LastName  = dto.LastName;
            user.Email     = dto.Email;
            user.Age       = dto.Age;

            return NoContent(); // 204
        }

        // DELETE api/users/{id}  - removes a user, returns 204
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = _store.GetById(id);
            if (user == null)
                return NotFound(new { message = $"No user found with id {id}." });

            _store.Remove(user);

            return NoContent(); // 204
        }
    }
}
