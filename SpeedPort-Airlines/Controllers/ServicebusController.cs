using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;

namespace SpeedPort_Airlines.Controllers
{
    public class ServicebusController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
