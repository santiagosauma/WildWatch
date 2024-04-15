using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace midas.Pages
{
    public class FormModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        public DateTime BirthDate { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "El género es obligatorio.")]
        public string Gender { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "La ciudad es obligatoria.")]
        public string City { get; set; }

        public string Message { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Message = "La entrada es inválida. Por favor, inténtalo de nuevo.";
                return Page();
            }

            var userIdString = HttpContext.Session.GetString("UserID");
            if (userIdString == null || !int.TryParse(userIdString, out int userId))
            {
                Message = "No se pudo recuperar el ID de usuario. Por favor, inicie sesión nuevamente.";
                return RedirectToPage("/Login");
            }

            string connectionString = "server=localhost;user=root;database=wildwatch;port=3306;password=";
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    var edad = DateTime.Today.Year - BirthDate.Year;
                    if (BirthDate.Date > DateTime.Today.AddYears(-edad)) edad--;

                    var query = @"
                        UPDATE Usuario
                        SET Edad = @Edad, Genero = @Gender, Localidad = @City
                        WHERE ID_Usuario = @UserID;";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", userId);
                        command.Parameters.AddWithValue("@Edad", edad);
                        command.Parameters.AddWithValue("@Gender", Gender);
                        command.Parameters.AddWithValue("@City", City);

                        await command.ExecuteNonQueryAsync();
                    }
                }
                Message = "Información guardada exitosamente.";
                return RedirectToPage("/User", new { id = userId });
            }
            catch (Exception ex)
            {
                Message = $"Error al guardar la información: {ex.Message}";
                return Page();
            }
        }
    }
}
