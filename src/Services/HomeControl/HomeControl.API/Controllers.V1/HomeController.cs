using Microsoft.AspNetCore.Mvc;
using HomeControl.API.Controllers.v1;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeControl.API.Controllers
{
    public class HomeController : BaseApiController
    {
        private readonly ILogger _logger;

        public HomeController(ILogger logger)
        {
            _logger = logger;
        }

       
        
    }
}
