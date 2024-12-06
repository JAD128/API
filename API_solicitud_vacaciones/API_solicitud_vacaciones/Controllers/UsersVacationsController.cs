using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace API_solicitud_vacaciones.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersVacationsController : Controller
    {
        private readonly string _connectionString = "server=CARITO_DELGADO\\MSSQLSERVER01;Database=vaciones_db;User id=sa;Password=12345;TrustServerCertificate=true;";

        [HttpPost("register")]

        public IActionResult Register([FromBody] UsersVacations user)
        {
            if (user == null)
            {
                return BadRequest("Invalid user data");
            }
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "insert into Usuarios(nombre, apellido, correo, telefono, nombre_g, correo_g, fecha_i, fecha_f, notas) values (@nombre, @apellido, @correo, @telefono, @nombre_g, @correo_g, @fecha_i, @fecha_f, @notas)";
                var rowAffected = connection.Execute(sql, new { user.nombre, user.apellido, user.correo, user.telefono, user.nombre_g, user.correo_g, user.fecha_i, user.fecha_f, user.notas});
                
                if (rowAffected > 0)
                {
                    return Ok("User registered successfully.");
                }
                else
                {
                    return StatusCode(500, "An error ocurred while registering the user.");
                }
            }

        }
    }
}
