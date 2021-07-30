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
        const string ServiceBusConnectionString = "Endpoint=sb://servicebusspeedportairlines.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=NkrTwbzG1EIoQz4YNnd29fjDw+z38O1bgHSCnSOb8HM="; 
        const string QueueName = "speedportqueue";

        public IActionResult Index()
        {
            return View();
        }
    }
}
