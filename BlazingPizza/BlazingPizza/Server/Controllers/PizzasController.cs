using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazingPizza.Server.Data;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlazingPizza.Server.Controllers
{
    [Route("[controller]"), ApiController]
    public class PizzasController : Controller
    {
        public PizzasController(PizzaContext context)
        {
            Context = context;
        }

        public PizzaContext Context { get; }
    }
}
