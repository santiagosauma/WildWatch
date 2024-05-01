﻿using System;
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
            var handler = new HttpClientHandler();
            // WARNING: Only use this in a development environment.
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            client = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://10.22.156.99:7026")
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public List<User> Users { get; set; }
        public User CurrentUser { get; set; }
        public List<Mistakes> Mistakes { get; set; }
        public List<MinigameScore> MinigameScores { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
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

    }
}
