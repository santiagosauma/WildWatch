using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace midas_api.Controllers
{
    [Route("api/[controller]")]
    public class AdminDashboardController : Controller
    {
        [HttpGet("CompletitionsByAge")]
        public IEnumerable<CompletitionsByAge> Get()
        {
            string connectionString = "Server=awaqdatabase-tec-932c.b.aivencloud.com;Port=12470;Database=wildwatch;Uid=avnadmin;password='AVNS_MRjSuICGDdluhdCYbor';";
            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;

            cmd.CommandText = "SELECT \n  RangoDeEdad,\n  COUNT(*) AS JugadoresConTodosMinijuegosCompletados\nFROM (\n  SELECT \n    U.ID_Usuario,\n    U.Edad,\n    CASE\n      WHEN U.Edad BETWEEN 18 AND 24 THEN '18-24 años'\n      WHEN U.Edad BETWEEN 25 AND 30 THEN '25-30 años'\n      WHEN U.Edad BETWEEN 31 AND 35 THEN '31-35 años'\n      WHEN U.Edad BETWEEN 36 AND 40 THEN '36-40 años'\n      ELSE '41 años en adelante'\n    END AS RangoDeEdad\n  FROM \n    Usuario U\n  JOIN (\n    SELECT \n      ID_Usuario_FK, \n      ID_CatalogoMinijuegos,\n      MAX(Puntaje) AS MaxPuntaje\n    FROM \n      Partida\n    WHERE Puntaje >= 80\n    GROUP BY \n      ID_Usuario_FK, ID_CatalogoMinijuegos\n  ) AS MejoresPuntajes ON U.ID_Usuario = MejoresPuntajes.ID_Usuario_FK\n  GROUP BY \n    U.ID_Usuario\n  HAVING \n    COUNT(DISTINCT MejoresPuntajes.ID_CatalogoMinijuegos) = 5 AND MIN(MejoresPuntajes.MaxPuntaje) >= 80\n) AS UsuariosConTodosMinijuegosCompletados\nGROUP BY \n  RangoDeEdad;";
            cmd.Prepare();

            CompletitionsByAge new_completition = new CompletitionsByAge();
            IList<CompletitionsByAge> CompletitionsList = new List<CompletitionsByAge>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    new_completition = new CompletitionsByAge();
                    new_completition.Range = reader["RangoDeEdad"].ToString();
                    new_completition.Completitions = Convert.ToInt32(reader["JugadoresConTodosMinijuegosCompletados"]);
                    CompletitionsList.Add(new_completition);
                }
            }


            conexion.Dispose();

            return CompletitionsList;
        }
    }
}

