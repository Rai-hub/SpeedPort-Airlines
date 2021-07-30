using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using SpeedPort_Airlines.Models;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Azure.ServiceBus.Core;

namespace SpeedPort_Airlines.Controllers
{
    public class ServicebusController : Controller
    {
        const string ServiceBusConnectionString = "Endpoint=sb://servicebusspeedportairlines.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=NkrTwbzG1EIoQz4YNnd29fjDw+z38O1bgHSCnSOb8HM="; 
        const string QueueName = "speedportqueue";

        public async Task<IActionResult> Index()
        {
            var managementClient = new ManagementClient(ServiceBusConnectionString);
            var queue = await managementClient.GetQueueRuntimeInfoAsync(QueueName);
            ViewBag.MessageCount = queue.MessageCount;
            return View();
        }

        [HttpPost] // after fill in the form         
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(BookPromoSlot bookpromoslot)
        {
            QueueClient queue = new QueueClient(ServiceBusConnectionString, QueueName);
            if (ModelState.IsValid)
            {
                var orderJSON = JsonConvert.SerializeObject(bookpromoslot);
                var message = new Message(Encoding.UTF8.GetBytes(orderJSON))
                {
                    MessageId = Guid.NewGuid().ToString(),
                    ContentType = "application/json"
                };
                await queue.SendAsync(message);
                return RedirectToAction("Index", "servicebus", new { });
            }
            return View(bookpromoslot);
        }

        private static async Task CreateQueueFunctionAsync()
        {
            var managementClient = new ManagementClient(ServiceBusConnectionString);
            bool queueExists = await managementClient.QueueExistsAsync(QueueName);
            if (!queueExists)
            {
                QueueDescription qd = new QueueDescription(QueueName);
                qd.MaxSizeInMB = 1024;
                qd.MaxDeliveryCount = 3;
                await managementClient.CreateQueueAsync(qd);
            }
        }

        public static void Initialize()
        {
            CreateQueueFunctionAsync().GetAwaiter().GetResult();
        }

        public async Task<ActionResult> ReceivedMessage()
        {
            var managementClient = new ManagementClient(ServiceBusConnectionString);
            var queue = await managementClient.GetQueueRuntimeInfoAsync(QueueName);
            List<BookPromoSlot> messages = new List<BookPromoSlot>();
            List<long> sequence = new List<long>();
            MessageReceiver messageReceiver = new MessageReceiver(ServiceBusConnectionString, QueueName);
            for (int i = 0; i < queue.MessageCount; i++)
            {
                Message message = await messageReceiver.PeekAsync();
                BookPromoSlot result = JsonConvert.DeserializeObject<BookPromoSlot>(Encoding.UTF8.GetString(message.Body));
                sequence.Add(message.SystemProperties.SequenceNumber);
                messages.Add(result);
            }
            ViewBag.sequence = sequence; ViewBag.messages = messages;
            return View();
        }

        public async Task<ActionResult> Approve(long sequence)
        {
            //connect to the same queue             
            var managementClient = new ManagementClient(ServiceBusConnectionString);
            var queue = await managementClient.GetQueueRuntimeInfoAsync(QueueName);
            //receive the selected message             
            MessageReceiver messageReceiver = new MessageReceiver(ServiceBusConnectionString, QueueName);
            BookPromoSlot result = null;
            for (int i = 0; i < queue.MessageCount; i++)
            {
                Message message = await messageReceiver.ReceiveAsync();
                string token = message.SystemProperties.LockToken;
                //to find the selected message - read and remove from the queue                 
                if (message.SystemProperties.SequenceNumber == sequence)
                {
                    result = JsonConvert.DeserializeObject<BookPromoSlot>(Encoding.UTF8.GetString(message.Body));
                    await messageReceiver.CompleteAsync(token);
                    break;
                }
            }
            return RedirectToAction("ReceiveMessaged", "servicebus");
        }
    }
}

