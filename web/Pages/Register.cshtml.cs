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
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(255, ErrorMessage = "El nombre no debe exceder los 255 caracteres.")]
        public string? Name { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        [MaxLength(255, ErrorMessage = "El correo electrónico no debe exceder los 255 caracteres.")]
        public string? Mail { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public string? Password { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Debe aceptar los términos y condiciones para continuar.")]
        public bool AcceptedTerms { get; set; }

        public string? Message { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Message = "Hay errores en el formulario. Por favor, revísalos.";
                return Page();
            }

            if (!AcceptedTerms)
            {
                Message = "Debe aceptar los términos y condiciones para registrarse.";
                return Page();
            }

            string connectionString = "{connectionStringSecret}";
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    var query = "INSERT INTO Usuario (email, clave, Is_Admin, Nombre, fecha_inicio) VALUES (@Mail, SHA2(@Password, 256), 0, @Name, CURDATE()); SELECT LAST_INSERT_ID();";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Mail", Mail);
                        command.Parameters.AddWithValue("@Password", Password);
                        command.Parameters.AddWithValue("@Name", Name);

                        var userId = await command.ExecuteScalarAsync();
                        HttpContext.Session.SetString("UserID", userId.ToString());
                        HttpContext.Session.SetString("isAdmin", "False");
                        return RedirectToPage("/Form");
                    }
                }
            }
            catch (Exception ex)
            {
                Message = $"Error durante el registro: {ex.Message}";
                return Page();
            }
        }
    }
}
