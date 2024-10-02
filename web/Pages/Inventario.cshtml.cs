using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using midas.Models;
using System.Data;

namespace midas.Pages
{
    public class InventarioModel : PageModel
    {
        public int? UserId { get; private set; }

        string _connectionString = "{connectionStringSecret}";

        public List<SerVivo> SeresVivos { get; set; } = new List<SerVivo>();

        public void OnGet()
        {
            var userIdString = HttpContext.Session.GetString("UserID");

            if (userIdString == null || !int.TryParse(userIdString, out int userId))
            {
                Response.Redirect("/Login");
            }
            else
            {
                UserId = userId;
            }
        }

        public async Task<IActionResult> OnGetCargarTodosAsync()
        {
            SeresVivos.Clear();
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string sql = "SELECT ID_CatalogoSeres, Nombre, Imagen FROM CatalogoSeres";
                using (var command = new MySqlCommand(sql, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            SeresVivos.Add(new SerVivo
                            {
                                ID = Convert.ToInt32(reader["ID_CatalogoSeres"]),
                                Nombre = reader["Nombre"].ToString(),
                                Imagen = reader["Imagen"].ToString()
                            });
                        }
                    }
                }
            }
            return new JsonResult(SeresVivos);
        }

        public async Task<IActionResult> OnGetSeresVivosPorCategoriaAsync(int categoriaId)
        {
            SeresVivos.Clear();
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = "SELECT ID_CatalogoSeres, Nombre, Imagen FROM CatalogoSeres WHERE ID_CatalogoMinijuegos = @CategoriaId";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@CategoriaId", categoriaId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            SeresVivos.Add(new SerVivo
                            {
                                ID = Convert.ToInt32(reader["ID_CatalogoSeres"]),
                                Nombre = reader["Nombre"].ToString(),
                                Imagen = reader["Imagen"].ToString()
                            });
                        }
                    }
                }
            }
            return new JsonResult(SeresVivos);
        }

        public async Task<IActionResult> OnGetBuscarAsync(string search)
        {
            SeresVivos.Clear();
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = "SELECT ID_CatalogoSeres, Nombre, Imagen FROM CatalogoSeres WHERE Nombre LIKE @SearchText";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@SearchText", $"%{search}%");
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            SeresVivos.Add(new SerVivo
                            {
                                ID = Convert.ToInt32(reader["ID_CatalogoSeres"]),
                                Nombre = reader["Nombre"].ToString(),
                                Imagen = reader["Imagen"].ToString()
                            });
                        }
                    }
                }
            }
            return new JsonResult(SeresVivos);
        }
    }
}
