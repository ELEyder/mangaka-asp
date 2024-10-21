using Mangaka.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json;

namespace Mangaka.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly MiDbContext _context;

        public LoginController(ILogger<LoginController> logger, MiDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] Login loginModel)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Datos Inválidos");
                }
                var user = await _context.User
                    .FirstOrDefaultAsync(u => u.Email == loginModel.Email && u.Password == loginModel.Password);

                if (user == null)
                {
                    return Unauthorized("Credenciales incorrectas: Usuario no encontrado");
                }

                string userDataJson = JsonSerializer.Serialize(user);
                HttpContext.Session.SetString("userDataJson", userDataJson);

                return Ok(new { redirectUrl = Url.Action("Index", "Home") });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en el proceso de autenticación.");
                return StatusCode(500, "Ocurrió un error en el servidor.");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
