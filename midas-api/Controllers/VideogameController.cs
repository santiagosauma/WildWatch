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
            string connectionString = "Server=awaqdatabase-tec-932c.b.aivencloud.com;Port=12470;Database=wildwatch;Uid=avnadmin;password='AVNS_MRjSuICGDdluhdCYbor';";
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

        [HttpGet("isTutorialCompleted/{id}")]
        public async Task<IActionResult> IsTutorialCompleted(int id)
        {
            string connectionString = "Server=awaqdatabase-tec-932c.b.aivencloud.com;Port=12470;Database=wildwatch;Uid=avnadmin;password='AVNS_MRjSuICGDdluhdCYbor';";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) AS GamesPlayed FROM Partida WHERE ID_Usuario_FK = @UserID;", connection))
                    {
                        cmd.Parameters.AddWithValue("@UserID", id);
                        cmd.Prepare();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                int gamesPlayed = Convert.ToInt32(reader["GamesPlayed"]);
                                bool isTutorialCompleted = gamesPlayed > 0;
                                return Json(new { isTutorialCompleted = isTutorialCompleted });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception (implement logging as needed)
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }

            return Json(new { isTutorialCompleted = false });
        }


        [HttpGet("MinigameScores/{id}")]
        public IEnumerable<MinigameScore> GetMinigameScoresByUserID(int id)
        {
            string connectionString = "Server=awaqdatabase-tec-932c.b.aivencloud.com;Port=12470;Database=wildwatch;Uid=avnadmin;password='AVNS_MRjSuICGDdluhdCYbor';";
            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;

            cmd.CommandText = "SELECT \n    cm.Tipo AS 'Minigame',\n    COALESCE(MAX(p.Puntaje), 0) AS 'Score'\nFROM \n    CatalogoMinijuegos cm\nLEFT JOIN (\n    SELECT \n        ID_CatalogoMinijuegos,\n        Puntaje\n    FROM \n        Partida\n    WHERE \n        ID_Usuario_FK = @UserID\n) p ON cm.ID_CatalogoMinijuegos = p.ID_CatalogoMinijuegos\nGROUP BY cm.ID_CatalogoMinijuegos\nORDER BY \n    cm.ID_CatalogoMinijuegos;";
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
            string connectionString = "Server=awaqdatabase-tec-932c.b.aivencloud.com;Port=12470;Database=wildwatch;Uid=avnadmin;password='AVNS_MRjSuICGDdluhdCYbor';";
            try
            {
                DateTime dateTime = DateTime.UtcNow.Date;
                using (var connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    var query = "INSERT INTO wildwatch.Partida (ID_Usuario_FK, ID_CatalogoMinijuegos, Fecha, Puntaje, Tiempo) VALUES (@UserID, @MinigameID, @Date, @Points, @Time);";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", minigameRequest.UserID);
                        command.Parameters.AddWithValue("@MinigameID", minigameRequest.MinigameID);
                        command.Parameters.AddWithValue("@Date", dateTime.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@Points", minigameRequest.Points);
                        command.Parameters.AddWithValue("@Time", minigameRequest.Time);

                        await command.ExecuteNonQueryAsync(); // Executes the insert command
                        var matchID = command.LastInsertedId; // Retrieves the ID of the inserted record

                        return Json(new { message = "Inserción correcta", MatchID = matchID });
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
