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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FormModel(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [BindProperty]
        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        public DateTime BirthDate { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "El g�nero es obligatorio.")]
        public string Gender { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "La ciudad es obligatoria.")]
        public string City { get; set; }

        [BindProperty]
        // No obligatorio
        public IFormFile ProfileImage { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "La identificacion es obligatoria.")]
        public IFormFile IDImage { get; set; }

        public string Message { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            /*
            if (!ModelState.IsValid)
            {
                Message = "La entrada es inv�lida. Por favor, int�ntalo de nuevo.";
                return Page();
            }
            */

            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

            // Ensure the directory exists
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Generate unique file names
            string uniqueFileNameProfile = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ProfileImage.FileName);
            string uniqueFileNameID = Guid.NewGuid().ToString() + "_" + Path.GetFileName(IDImage.FileName);

            // Save profile image
            var profileImagePath = Path.Combine(uploadsFolder, uniqueFileNameProfile);
            using (var fileStream = new FileStream(profileImagePath, FileMode.Create))
            {
                await ProfileImage.CopyToAsync(fileStream);
            }

            // Save ID image
            var idImagePath = Path.Combine(uploadsFolder, uniqueFileNameID);
            using (var fileStream = new FileStream(idImagePath, FileMode.Create))
            {
                await IDImage.CopyToAsync(fileStream);
            }

            var userIdString = HttpContext.Session.GetString("UserID");
            if (userIdString == null || !int.TryParse(userIdString, out int userId))
            {
                Message = "No se pudo recuperar el ID de usuario. Por favor, inicie sesi�n nuevamente.";
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
                        SET FotoPerfil = @ProfileImage, FotoID = @IDImage, Edad = @Edad, Genero = @Gender, Localidad = @City
                        WHERE ID_Usuario = @UserID;";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", userId);
                        command.Parameters.AddWithValue("@Edad", edad);
                        command.Parameters.AddWithValue("@Gender", Gender);
                        command.Parameters.AddWithValue("@City", City);
                        command.Parameters.AddWithValue("@ProfileImage", profileImagePath);
                        command.Parameters.AddWithValue("@IDImage", idImagePath);

                        await command.ExecuteNonQueryAsync();
                    }
                }
                Message = "Informaci�n guardada exitosamente.";
                return RedirectToPage("/User", new { id = userId });
            }
            catch (Exception ex)
            {
                Message = $"Error al guardar la informaci�n: {ex.Message}";
                return Page();
            }
        }
    }
}
