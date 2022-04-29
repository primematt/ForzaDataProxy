using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ForzaDataProxy.Controllers
{

    [ApiController]
    [Route("/")]
    public class DataController : Controller
    {
        private readonly IConfiguration Config;
        private readonly ILogger<DataController> Logger;

        public DataController(IConfiguration config, ILogger<DataController> logger)
        {
            Config = config;
            Logger = logger;
        }

        [HttpGet()]
        public async Task<IActionResult> Index()
        {
            return View();
        }


    }
}