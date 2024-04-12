using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace midas.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        [Required, MaxLength(255)]
        public string? Name { get; set; }

        [BindProperty]
        [Required, EmailAddress, MaxLength(255)]
        public string? Mail { get; set; }

        [BindProperty]
        [Required, DataType(DataType.Password), MinLength(6)]
        public string? Password { get; set; }

        [BindProperty]
        public int? Age { get; set; } = 0; // Valor predeterminado

        [BindProperty]
        public string? Gender { get; set; } = "No especificado"; // Valor predeterminado

        [BindProperty]
        public string? Locality { get; set; } = "No especificada"; // Valor predeterminado

        public string? Message { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            // Eliminar verificaciones de campos que no son estrictamente necesarios
            ModelState.Remove("Gender");
            ModelState.Remove("Locality");

            if (!ModelState.IsValid)
            {
                Message = "La entrada es inv�lida. Por favor, int�ntalo de nuevo.";
                return Page();
            }

            string connectionString = "server=localhost;user=root;database=wildwatch;port=3306;password=";
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    var query = "INSERT INTO Usuario (email, clave, Is_Admin, Nombre, fecha_inicio, Edad, Genero, Localidad) VALUES (@Mail, SHA2(@Password, 256), 0, @Name, CURDATE(), @Age, @Gender, @Locality); SELECT LAST_INSERT_ID();";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Mail", Mail);
                        command.Parameters.AddWithValue("@Password", Password);
                        command.Parameters.AddWithValue("@Name", Name);
                        command.Parameters.AddWithValue("@Age", Age);
                        command.Parameters.AddWithValue("@Gender", Gender);
                        command.Parameters.AddWithValue("@Locality", Locality);

                        var userId = await command.ExecuteScalarAsync();
                        HttpContext.Session.SetString("UserID", userId.ToString());
                    }
                }
                Message = "Registro exitoso. �Bienvenido a nuestra comunidad!";
                return RedirectToPage("/Form");
            }
            catch (Exception ex)
            {
                Message = $"Error durante el registro: {ex.Message}";
                return Page();
            }
        }
    }
}
