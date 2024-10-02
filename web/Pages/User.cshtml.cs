using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using midas.Models;
using Newtonsoft.Json;

namespace midas.Pages
{
	public class UserModel : PageModel
    {
        private static readonly HttpClient client;

        // Constructor
        static UserModel()
        {
            var handler = new HttpClientHandler();
            // WARNING: Only use this in a development environment.
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            client = new HttpClient(handler)
            {
                // BaseAddress = new Uri("https://localhost:7026")
                BaseAddress = new Uri("https://10.22.156.99:7026")
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public int? UserId { get; private set; }

        public User CurrentUser { get; set; }
        public List<Leaderboard> Leaderboard { get; set; }
        public List<MinigameScore> MinigameScores { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var userIdString = HttpContext.Session.GetString("UserID");
            if (userIdString == null || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToPage("/Login");
            }
            else
            {
                UserId = userId;
                if(userIdString != null && UserId != id)
                {
                    return RedirectToPage("/User", new { id = UserId });
                }
            }

            // Send request to https://10.22.156.99:7026/api/UserDashboard/{id} to fill data of a single User

            CurrentUser = await GetUsersAsync(id);
            Leaderboard = await GetLeadearboardAsync();
            MinigameScores = await GetMinigameScores(id);

            return Page();
        }

        public async Task<User> GetUsersAsync(int id)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("api/UserDashboard/"+id.ToString());
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

        public async Task<List<MinigameScore>> GetMinigameScores(int id)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("api/UserDashboard/MinigameScores/"+id.ToString());
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
      
        public async Task<List<Leaderboard>> GetLeadearboardAsync()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("api/UserDashboard/Leaderboard");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Leaderboard>>(jsonString);
                }
            }
            catch (Exception e)
            {
                Console.Write("Error: " + e.Message);
            }
            return new List<Leaderboard>();
        }
    }
}
