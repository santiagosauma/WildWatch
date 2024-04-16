using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace midas_api.Controllers
{
    [Route("api/[controller]")]
    public class UserDashboardController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<User> Get()
        {
            string connectionString = "Server=127.0.0.1;Port=3306;Database=wildwatch;Uid=root;password='';";
            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;

            // CHANGE QUERY
            cmd.CommandText = "SELECT \n    u.ID_Usuario, u.Nombre, \n    u.fecha_inicio, \n    u.Edad,\n    u.Genero,\n    u.Localidad,\n    (COUNT(DISTINCT CASE WHEN p.Puntaje >= 80 THEN p.ID_CatalogoMinijuegos ELSE NULL END) * 20) AS `progreso`\nFROM \n    Usuario u\nINNER JOIN \n    Partida p ON u.ID_Usuario = p.ID_Usuario_FK\nGROUP BY \n    u.ID_Usuario, u.Nombre, u.fecha_inicio, u.Edad, u.Genero, u.Localidad;\n";
            cmd.Prepare();

            User new_user = new User();
            IList<User> UserList = new List<User>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    new_user = new User();
                    new_user.ID = Convert.ToInt32(reader["ID_Usuario"]);
                    new_user.Progress = Convert.ToInt32(reader["progreso"]);
                    new_user.Name = reader["nombre"].ToString();
                    new_user.Date = reader["fecha_inicio"].ToString();
                    new_user.Age = reader["Edad"].ToString();
                    new_user.Location = reader["Localidad"].ToString();
                    new_user.Gender = reader["Genero"].ToString();
                    new_user.ProfilePicture = reader["FotoPerfil"].ToString();
                    new_user.IdPicture = reader["FotoID"].ToString();
                }
            }


            conexion.Dispose();

            return UserList;
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetUserById(int id)
        {
            string connectionString = "Server=127.0.0.1;Port=3306;Database=wildwatch;Uid=root;password='';";
            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;

            // CHANGE QUERY
            cmd.CommandText = "SELECT \n    u.ID_Usuario, u.FotoPerfil, u.FotoID, u.Nombre, \n    u.fecha_inicio, \n    u.Edad,\n    u.Genero,\n    u.Localidad\nFROM \n    Usuario u\nWHERE \n    u.ID_Usuario = @UserID; -- Replace @UserID with the actual ID of the user.\n";
            cmd.Parameters.AddWithValue("@UserID", id);
            cmd.Prepare();

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    var user = new User
                    {
                        ID = Convert.ToInt32(reader["ID_Usuario"]),
                        Name = reader["nombre"].ToString(),
                        Date = reader["fecha_inicio"].ToString(),
                        Age = reader["Edad"].ToString(),
                        Location = reader["Localidad"].ToString(),
                        Gender = reader["Genero"].ToString(),
                        ProfilePicture = reader["FotoPerfil"].ToString(),
                        IdPicture = reader["FotoID"].ToString()
                    };

                    return user;

                }
            }


            conexion.Dispose();

            return NotFound("No User Found");
        }

        [HttpGet("MinigameScores/{id}")]
        public IEnumerable<MinigameScore> GetMinigameScoresByUserID(int id)
        {
            string connectionString = "Server=127.0.0.1;Port=3306;Database=wildwatch;Uid=root;password='';";
            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;

            // CHANGE QUERY
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
                    new_minigame.Mistakes = Convert.ToInt32(reader["Mistakes"]);
                    new_minigame.Time = Convert.ToInt32(reader["Time"]);
                    MinigameScoreList.Add(new_minigame);
                }
            }


            conexion.Dispose();

            return MinigameScoreList;
        }


        [HttpGet("Leaderboard")]
        public IEnumerable<Leaderboard> GetLeaderboard()
        {
            string connectionString = "Server=127.0.0.1;Port=3306;Database=wildwatch;Uid=root;password='';";
            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;

            // CHANGE QUERY
            cmd.CommandText = "SELECT \n    u.Nombre AS Usuario, \n    SUM(max_scores.MaxPuntaje) AS Puntaje, \n    u.Localidad AS Localidad \nFROM \n    Usuario u\nJOIN (\n    SELECT \n        ID_Usuario_FK, \n        MAX(Puntaje) AS MaxPuntaje\n    FROM \n        Partida\n    GROUP BY \n        ID_Usuario_FK, ID_CatalogoMinijuegos\n) AS max_scores ON u.ID_Usuario = max_scores.ID_Usuario_FK\nGROUP BY \n    u.ID_Usuario, u.Nombre, u.Localidad\nORDER BY \n    Puntaje DESC\nLIMIT 5";
            cmd.Prepare();

            Leaderboard new_leaderboard = new Leaderboard();
            IList<Leaderboard> LeadearboardList = new List<Leaderboard>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    new_leaderboard = new Leaderboard();
                    new_leaderboard.Location = reader["Localidad"].ToString();
                    new_leaderboard.Name = reader["Usuario"].ToString();
                    new_leaderboard.Score = Convert.ToInt32(reader["Puntaje"]);
                    LeadearboardList.Add(new_leaderboard);
                }
            }


            conexion.Dispose();

            return LeadearboardList;
        }


    }
}

