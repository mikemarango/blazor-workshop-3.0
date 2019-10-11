using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BlazingPizza.Shared;
using Microsoft.AspNetCore.Components;

namespace BlazingPizza.Client.Services
{
    public class PizzaClient
    {
        private readonly HttpClient httpClient;

        public PizzaClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public Task<List<PizzaSpecial>> GetSpecialsAsync()
        {
            return httpClient.GetJsonAsync<List<PizzaSpecial>>("specials");
        }

        public Task<List<Topping>> GetToppingsAsync()
        {
            return httpClient.GetJsonAsync<List<Topping>>("toppings");
        }
    }
}
