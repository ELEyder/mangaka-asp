using Mangaka.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json;

namespace Mangaka.Controllers
{
    public class RegisterController : Controller
    {
        public readonly MiDbContext _context;
        private readonly ILogger<RegisterController> _logger;
        public RegisterController(MiDbContext context, ILogger<RegisterController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] Register registerModel)
        {
                if (!ModelState.IsValid) return BadRequest("Datos Inválidos");
                var user = await _context.User
                    .FirstOrDefaultAsync(u => u.Email == registerModel.Email);

                if (user != null) return Unauthorized("Correo ya Registrado");
                if (registerModel.Password != registerModel.RepeatPassword) return BadRequest("La contraseña no es la misma");

                User newUser = new User
                {
                    Name = registerModel.Username,
                    Email = registerModel.Email,
                    Password = registerModel.Password,
                    DateCreate = DateTime.Now,
                    Status = true,
                };
                _context.User.Add(newUser);
                await _context.SaveChangesAsync();

                string userDataJson = JsonSerializer.Serialize(newUser);
                HttpContext.Session.SetString("userDataJson", userDataJson);

                return Ok(new { redirectUrl = Url.Action("Index", "Home") });
        }
    }
}
