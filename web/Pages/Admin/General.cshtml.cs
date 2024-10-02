using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using midas.Models;
using Newtonsoft.Json;
using static Plotly.NET.StyleParam.DrawingStyle;

namespace midas.Pages.Admin
{
	public class GeneralModel : PageModel
    {
        private static readonly HttpClient client;
        static GeneralModel ()
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

        public List<CompletitionsByAge> CompletitionsByAge { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdString = HttpContext.Session.GetString("UserID");
            var isAdmin = HttpContext.Session.GetString("isAdmin");

            if (userIdString == null || !int.TryParse(userIdString, out int userId))
            {
                Response.Redirect("/Login");
            } else
            {
                if (isAdmin == "False")
                {
                    return RedirectToPage("/User", new { id = userId });
                }
            }      

            CompletitionsByAge = await GetCompletitionsAsync();

            return Page();
        }
        public async Task<List<CompletitionsByAge>> GetCompletitionsAsync()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("api/AdminDashboard/CompletitionsByAge");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<CompletitionsByAge>>(jsonString);
                }
            }
            catch (Exception e)
            {
                Console.Write("Error: " + e.Message);
            }
            return new List<CompletitionsByAge>();
        }
    }
}
