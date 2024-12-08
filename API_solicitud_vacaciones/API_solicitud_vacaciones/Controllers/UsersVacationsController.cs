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
                var sql = "insert into Usuarios(name,lastName,email,phone,name_g,email_g,date_ini,date_fi,notes) values (@name,@lastName,@email,@phone,@name_g,@email_g,@date_ini,@date_fi,@notes)";
                var rowAffected = connection.Execute(sql, new { user.name, user.lastName, user.email, user.phone, user.name_g, user.email_g, user.date_ini, user.date_fi, user.notes});
                
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

        [HttpPost("validation")]

        public IActionResult Validation([FromBody] UsersValidation user)
        {
            if (user == null)
            {
                return BadRequest("Invalid user data");
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sql = "select * from Usuarios where lastName=@lastName and email=@email;";
                var result = connection.QuerySingleOrDefault<UsersValidation>(sql, new { user.lastName, user.email });

                if (result != null)
                {
                    return Ok("User found");
                }
                else
                {
                    return Unauthorized("Invalid credentials");
                }
            }
        }
    }
}
