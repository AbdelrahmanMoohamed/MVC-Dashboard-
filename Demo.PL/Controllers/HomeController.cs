using Demo.PL.Models;
using Demo.PL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IScopedService scoped1;
        private readonly IScopedService scoped2;
        private readonly ITransientService transient1;
        private readonly ITransientService transient2;
        private readonly ISingeltonService singelton1;
        private readonly ISingeltonService singelton2;

        public HomeController(ILogger<HomeController> logger
            ,IScopedService scoped1
            ,IScopedService scoped2
                ,ITransientService transient1
                ,ITransientService transient2
            ,ISingeltonService singelton1
            ,ISingeltonService singelton2
            ) // ASK CLR Create Object From Class Implement ILogger
        {
            _logger = logger;
            this.scoped1 = scoped1;
            this.scoped2 = scoped2;
            this.transient1 = transient1;
            this.transient2 = transient2;
            this.singelton1 = singelton1;
            this.singelton2 = singelton2;
            this.singelton2 = singelton2;
        }


        // /Home/TestLifeTime
        public string TestLifeTime()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append($"Scoped01 :: {scoped1.GetGuid()}\n");
            builder.Append($"Scoped02 :: {scoped2.GetGuid()}\n\n");
            builder.Append($"Transient1 :: {transient1.GetGuid()}\n");
            builder.Append($"Transient2 :: {transient2.GetGuid()}\n\n");
            builder.Append($"Singelton1 :: {singelton1.GetGuid()}\n");
            builder.Append($"Singelton2 :: {singelton2.GetGuid()}\n\n");

            return builder.ToString();
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
