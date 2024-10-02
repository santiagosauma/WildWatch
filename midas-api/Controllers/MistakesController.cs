using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace midas_api.Controllers
{
    [Route("api/[controller]")]
    public class MistakesController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public IEnumerable<Mistakes> Get(int id)
        {
            string connectionString = "Server=awaqdatabase-tec-932c.b.aivencloud.com;Port=12470;Database=wildwatch;Uid=avnadmin;password='AVNS_MRjSuICGDdluhdCYbor';";
            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;

            cmd.CommandText = "SELECT \n    e.Tipo AS `Equivocaciones`, \n    cm.Tipo AS `Minijuego`, \n    e.Descripcion\nFROM \n    Errores e\nINNER JOIN \n    Partida_Errores pe ON e.ID_Errores = pe.ID_Errores\nINNER JOIN \n    Partida p ON pe.ID_Partida = p.ID_Partida\nINNER JOIN \n    Usuario u ON p.ID_Usuario_FK = u.ID_Usuario\nINNER JOIN \n    CatalogoMinijuegos cm ON p.ID_CatalogoMinijuegos = cm.ID_CatalogoMinijuegos\nWHERE \n    u.ID_Usuario = @UserID; -- Replace @UserID with the actual ID of the user.\n";
            cmd.Parameters.AddWithValue("@UserID", id);
            cmd.Prepare();

            Mistakes new_mistakes = new Mistakes();
            IList<Mistakes> MistakesList = new List<Mistakes>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    new_mistakes = new Mistakes();
                    new_mistakes.Description = reader["Descripcion"].ToString();
                    new_mistakes.Minigame = reader["Minijuego"].ToString();
                    new_mistakes.Type = reader["Equivocaciones"].ToString();
                    MistakesList.Add(new_mistakes);
                }
            }


            conexion.Dispose();

            return MistakesList;
        }
    }
}

