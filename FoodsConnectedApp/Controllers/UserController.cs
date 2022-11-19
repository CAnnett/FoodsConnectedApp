using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodsConnectedApp.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace FoodsConnectedApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserContext _context;

        public UserController(UserContext context)
        {
            _context = context;
        }

        //GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.OrderBy(e => e.Id).ToListAsync();
        }

        //GET: api/Users/<id>
        //Extra get request to get a singular user
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        //PUT: api/Users/<id>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {

            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;
            bool containsItem = _context.Users.Any(item => item.Username == user.Username);

            if (containsItem)
            {
                throw new Exception("Username already taken.");
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        //POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            bool containsItem = _context.Users.Any(item => item.Username == user.Username);
            if (containsItem)
            {
                throw new Exception("Username already taken.");
            }
            else
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

            }

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        //DELETE api/Users/<id>
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        //Check if user exists
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }


    }
}