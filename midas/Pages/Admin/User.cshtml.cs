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

namespace midas.Pages.Admin
{
	public class UserModel : PageModel
    {
        private static readonly HttpClient client;

        // Constructor
        static UserModel()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7026")
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public List<User> Users { get; set; }
        public List<Mistakes> Mistakes { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Send request to https://localhost:7026/api/UserDashboard to fill data

            Users = await GetUsersAsync();
           
            return Page();
        }

        public async Task<List<User>> GetUsersAsync()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("api/UserDashboard");
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
    }
}
