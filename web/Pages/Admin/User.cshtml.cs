using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using midas.Models;
using MySql.Data.MySqlClient;

namespace midas.Pages.Admin
{
	public class UserModel : PageModel
    {
        private static readonly HttpClient client;

        static UserModel()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            client = new HttpClient(handler)
            {
                // BaseAddress = new Uri("https://localhost:7026")
                BaseAddress = new Uri("https://10.22.156.99:7026")
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public List<User> Users { get; set; }
        public User CurrentUser { get; set; }
        public List<Mistakes> Mistakes { get; set; }
        public List<MinigameScore> MinigameScores { get; set; }

        public string? Message { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var userIdString = HttpContext.Session.GetString("UserID");
            var isAdmin = HttpContext.Session.GetString("isAdmin");

            if (userIdString == null || !int.TryParse(userIdString, out int userId))
            {
                Response.Redirect("/Login");
            }
            else
            {
                if (isAdmin == "False")
                {
                    return RedirectToPage("/User", new { id = userId });
                }
            }

            Message = TempData["Message"] as string;

            Users = await GetUsersAsync();
            if (id.HasValue)
            {
                Mistakes = await GetMistakesById(id.Value);
                MinigameScores = await GetMinigameScores(id.Value);
                CurrentUser = await GetUserById(id.Value);
            }

            return Page();
        }

        public async Task<List<User>> GetUsersAsync()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("api/UserDashboard/");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<User>>(jsonString);
                }
            }
            catch (Exception e)
            {
                Console.Write("Error: " + e.Message);
            }
            return new List<User>();
        }

        public async Task<List<Mistakes>> GetMistakesById(int id)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("api/Mistakes/" + id.ToString());
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Mistakes>>(jsonString);
                }
            }
            catch (Exception e)
            {
                Console.Write("Error: " + e.Message);
            }
            return new List<Mistakes>();
        }

        public async Task<List<MinigameScore>> GetMinigameScores(int id)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("api/UserDashboard/MinigameScores/" + id.ToString());
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<MinigameScore>>(jsonString);
                }
            }
            catch (Exception e)
            {
                Console.Write("Error: " + e.Message);
            }
            return new List<MinigameScore>();
        }

        public async Task<User> GetUserById(int id)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("api/UserDashboard/" + id.ToString());
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<User>(jsonString);
                }
            }
            catch (Exception e)
            {
                Console.Write("Error: " + e.Message);
            }
            return new User();
        }

        public async Task<IActionResult> OnPostAsync(int? userId)
        {
            if (userId == null)
            {
                TempData["Message"] = "No se pudo recuperar el ID de usuario";
                Message = "No se pudo recuperar el ID de usuario";
                return RedirectToPage();
            }

            string connectionString = "{connectionStringSecret}";
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    var query = @"
                UPDATE Usuario
                SET is_Active = 0
                WHERE ID_Usuario = @UserID;";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", userId);
                        await command.ExecuteNonQueryAsync();
                    }
                }
                TempData["Message"] = "Usuario deshabilitado exitosamente";
                return RedirectToPage("/Admin/User", new { id = "" });
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Error al guardar la información: {ex.Message}";
                return RedirectToPage();
            }
        }

    }
}
