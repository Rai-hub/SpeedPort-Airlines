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
            CloudBlobContainer container = clientAgent.GetContainerReference("testblob");

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

        //upload multiple images to the blob storage
        public string uploadmultipleimages()
        {
            CloudBlobContainer container = getcontainerinformation();

            string filename = ""; string message = "";
            //assume upload 3 images at the same time
            for (int i = 1; i <= 4; i++)
            {
                CloudBlockBlob blobitem = container.GetBlockBlobReference("image" + i + ".jpg"); //path.getextension()

                try
                {
                    using (var fileStream = System.IO.File.OpenRead(@"C:\\Users\\doosh\\source\\repos\\SpeedPort-Airlines\\SpeedPort-Airlines\\wwwroot\\Images\\Banners\\Banner" + i + ".jpg"))
                    {
                        filename = fileStream.Name;
                        blobitem.UploadFromStreamAsync(fileStream).Wait();
                    }
                    message = message + filename + " is already uploaded in the blob storage! \n";
                }
                catch (Exception ex)
                {
                    return message + "\nTechnical issue : " + ex.ToString() + ". Please try to upload " + filename + "again!";
                }
            }

                return message;
        }

        //Display all the images as a picture gallery (view)
        public ActionResult ViewBanner(string message = null)
        {
            ViewBag.msg = message;

            CloudBlobContainer container = getcontainerinformation();

            //create a new empty list to contain the blobs information
            List<string> blobitems = new List<string>();

            //read the blob storage items using below code
            BlobResultSegment result = container.ListBlobsSegmentedAsync(null).Result;

            //split one by one items from the list
            foreach (IListBlobItem item in result.Results)
            {
                //blob type = block blob/ append blob/ directory
                if (item.GetType() == typeof(CloudBlockBlob)) //filter the blob type
                {
                    CloudBlockBlob singleblob = (CloudBlockBlob)item;
                    //block blob = video / audio / images (jpg /png/ gif)
                    if (Path.GetExtension(singleblob.Name.ToString()) == ".jpg")
                    {
                        //add item info to list<string>
                        blobitems.Add(singleblob.Name + "#" + singleblob.Uri.ToString());
                    }
                }
            }
            return View(blobitems);
        }
    }



}
