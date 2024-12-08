using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace API_front.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly string _connectionString = "server=CARITO_DELGADO\\MSSQLSERVER01;Database=dbtest;User id=sa;Password=12345;TrustServerCertificate=true;";
        
        [HttpPost("login")]

        public IActionResult Login([FromBody] Users user)
        // Método de accion en un controlador de ASP.NET Core que recibe datos de una solicitud HTTP y devuelve un resultado de acción.
        // Indica que el método espera un objeto user tipo User en el cuerpo de la solicitud HTTP. El atributo [FromBody]
        // le dice APS.NET Core que los datos deben ser deserializados desde el cuerpo de la solicitud en un objeto Users
        {
            if (user == null)
            {
                return BadRequest("Invalid user data.");
            }
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "select * from Users where username = @username and password = @password";
                var result = connection.QuerySingleOrDefault<Users>(sql, new { user.username, user.password }); // realizar los datos capoturados
                if (result != null)
                {
                    return Ok("Login successful.");
                }
                else
                {
                    return Unauthorized("Unvalid credentials.");
                }
            }
        }

        [HttpPost("register")]

        public IActionResult Register([FromBody] Users user)
        {
            if (user == null)
            {
                return BadRequest("Invalid user dara");
            }
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "insert into Users (username, password) values (@username, @password)";
                var rowAffected = connection.Execute(sql, new { user.username, user.password });

                if (rowAffected > 0)
                {
                    return Ok("User registered successfully.");
                }
                else
                {
                    return StatusCode(500,"An error ocurred while registering the user.");
                }
            }
        }
    }
}
