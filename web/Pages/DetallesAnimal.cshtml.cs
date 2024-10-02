using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using midas.Models;
using System.Data;

namespace midas.Pages
{
    public class DetallesAnimalModel : PageModel
    {
        public int? UserId { get; private set; }

        string _connectionString = "{connectionStringSecret}";

        public SerVivo Animal { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
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

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = "SELECT ID_CatalogoSeres, Nombre, NombreCientifico, Imagen, Descripcion, Sonido FROM CatalogoSeres WHERE ID_CatalogoSeres = @Id";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            Animal = new SerVivo
                            {
                                ID = reader.GetInt32("ID_CatalogoSeres"),
                                Nombre = reader.GetString("Nombre"),
                                NombreCientifico = reader.GetString("NombreCientifico"),
                                Imagen = reader.GetString("Imagen"),
                                Descripcion = reader.GetString("Descripcion"),
                                Sonido = reader.IsDBNull(reader.GetOrdinal("Sonido")) ? null : reader.GetString("Sonido")
                            };
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                }
            }
            return Page();
        }
    }
}
