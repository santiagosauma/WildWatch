using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace midas_api.Controllers
{
    [Route("api/[controller]")]
    public class VideogameController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LoginRequest loginRequest)
        {
            string connectionString = "server=localhost;user=root;database=wildwatch;port=3306;password=";
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    var query = "SELECT ID_Usuario, Is_Active FROM Usuario WHERE email = @Mail AND clave = SHA2(@Password, 256)";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Mail", loginRequest.Mail);
                        command.Parameters.AddWithValue("@Password", loginRequest.Password);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                var userId = reader["ID_Usuario"].ToString();
                                byte[] isActiveBytes = (byte[])reader["Is_Active"];
                                bool isActive = isActiveBytes[0] == 49; 
                                if (isActive)
                                {
                                    return Json(new { Success = true, UserID = userId });
                                } else
                                {
                                    return StatusCode(403, "Cuenta deshabilitada. Contacte al administrador."); // HTTP 403 Forbidden
                                }
                            }
                            else
                            {
                                return BadRequest("Correo o contraseña incorrectos."); // HTTP 400 Bad Request
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error en base de datos: {ex.Message}"); // HTTP 500 Internal Server Error
            }
        }

        [HttpGet("MinigameScores/{id}")]
        public IEnumerable<MinigameScore> GetMinigameScoresByUserID(int id)
        {
            string connectionString = "Server=127.0.0.1;Port=3306;Database=wildwatch;Uid=root;password='';";
            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;

            cmd.CommandText = "SELECT \n    cm.Tipo AS 'Minigame',\n    COALESCE(p.Puntaje, 0) AS 'Score',\n    COALESCE((\n        SELECT COUNT(*)\n        FROM Partida_Errores\n        WHERE ID_Partida = p.ID_Partida\n    ), 0) AS 'Mistakes',\n    COALESCE((\n        SELECT SUM(Tiempo)\n        FROM Partida\n        WHERE ID_Usuario_FK = @UserID AND ID_CatalogoMinijuegos = cm.ID_CatalogoMinijuegos\n    ), 0) AS 'Time'\nFROM \n    CatalogoMinijuegos cm\nLEFT JOIN (\n    SELECT \n        ID_Partida,\n        ID_Usuario_FK,\n        ID_CatalogoMinijuegos,\n        Puntaje,\n        Fecha\n    FROM \n        Partida\n    WHERE \n        ID_Usuario_FK = @UserID AND\n        Fecha IN (\n            SELECT MAX(Fecha) \n            FROM Partida \n            WHERE ID_Usuario_FK = @UserID\n            GROUP BY ID_CatalogoMinijuegos\n        )\n) p ON cm.ID_CatalogoMinijuegos = p.ID_CatalogoMinijuegos\nORDER BY \n    cm.ID_CatalogoMinijuegos;\n";
            cmd.Parameters.AddWithValue("@UserID", id);
            cmd.Prepare();

            MinigameScore new_minigame = new MinigameScore();
            IList<MinigameScore> MinigameScoreList = new List<MinigameScore>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    new_minigame = new MinigameScore();
                    new_minigame.Minigame = reader["Minigame"].ToString();
                    new_minigame.Score = Convert.ToInt32(reader["Score"]);
                    MinigameScoreList.Add(new_minigame);
                }
            }


            conexion.Dispose();

            // Probablemente se ocupe Json()
            return MinigameScoreList;
        }

        [HttpPost("Minigame")]
        public async Task<IActionResult> PostMinigameData([FromBody] MinigameRequest minigameRequest)
        {
            string connectionString = "server=localhost;user=root;database=wildwatch;port=3306;password=";
            try
            {
                DateTime dateTime = DateTime.UtcNow.Date;
                using (var connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    var query = "INSERT INTO wildwatch.Partida\n(ID_Usuario_FK, ID_CatalogoMinijuegos, Fecha, Puntaje, Tiempo)\nVALUES(@UserID, @MinigameID, @Date, @Points, @Time);";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", minigameRequest.UserID);
                        command.Parameters.AddWithValue("@MinigameID", minigameRequest.MinigameID);
                        command.Parameters.AddWithValue("@Date", dateTime.ToString("yyyy/MM/dd"));
                        command.Parameters.AddWithValue("@Points", minigameRequest.Points);
                        command.Parameters.AddWithValue("@Time", minigameRequest.Time);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var matchID = await command.ExecuteScalarAsync();
                            return Json(new { message = "Insercion correcta" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error en base de datos: {ex.Message}"); // HTTP 500 Internal Server Error
            }
        }

    }

}
