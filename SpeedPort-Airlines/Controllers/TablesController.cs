using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SpeedPort_Airlines.Controllers
{
    public class TablesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private CloudTable getStorageAccountInformation()
        {
            //1.1 link with the appsettings.json to get the accesskey
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json");
            IConfigurationRoot configure = builder.Build();
            CloudStorageAccount accountobject = CloudStorageAccount.Parse(configure["ConnectionStrings:blobstorageconnection"]);

            //1.2 create/link to related table
            CloudTableClient tableclient = accountobject.CreateCloudTableClient();
            CloudTable table = tableclient.GetTableReference("TestTable");
            return table;
        }

        public ActionResult CreateTable()
        {
            CloudTable table = getStorageAccountInformation();
            //create table if table not yet exist
            ViewBag.Success = table.CreateIfNotExistsAsync().Result;
            ViewBag.tablename = table.Name;

            //display the create table result
            return View();
        }
    }
}
