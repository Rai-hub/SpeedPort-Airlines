using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SpeedPort_Airlines.Controllers
{
    public class BlobsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private CloudBlobContainer getcontainerinformation()
        {
            //link with the appsettings.json to get the accesskey
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json");
            IConfigurationRoot configure = builder.Build();
            
            //to get the key access fromt he appsettings.json
            CloudStorageAccount accountdetails = CloudStorageAccount
                .Parse(configure["ConnectionStrings:blobstorageconnection"]);

            //give info about which container name actually you wish to refer/create
            CloudBlobClient clientAgent = accountdetails.CreateCloudBlobClient();
            CloudBlobContainer container = clientAgent.GetContainerReference("TestBlob");

            return container;
        }

        //this method used for creating the container
        public IActionResult CreateNewContainer()
        {
            //get container information and its accesskey to do creation action.
            CloudBlobContainer container = getcontainerinformation();

            //create the container if the system found that the name is not exist in the list
            ViewBag.Success = container.CreateIfNotExistsAsync().Result;

            //collect the container name to display in the frontend
            ViewBag.containername = container.Name;

            //modify view and display the result to the user
            return View();
        }
    }
}
