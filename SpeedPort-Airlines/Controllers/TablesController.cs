using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SpeedPort_Airlines.Models;

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

        public ActionResult InsertSingleData()
        {
            CloudTable table = getStorageAccountInformation();

            //object for DestinationCountryEntity 1
            DestinationCountryEntity SpecialFlightDeal1 = new DestinationCountryEntity("England", "London");
            SpecialFlightDeal1.Season = "Summer";
            SpecialFlightDeal1.PromoExpiry = new DateTime (2021, 6, 01);
            SpecialFlightDeal1.PromoCode = "FLYLONDON22";

            try
            {
                //insert operation here
                TableOperation insert = TableOperation.Insert(SpecialFlightDeal1);
                TableResult result = table.ExecuteAsync(insert).Result;

                //get the result answer (success / not success to insert)
                ViewBag.TableName = table.Name;
                ViewBag.result = result.HttpStatusCode;
            }
            catch (Exception ex)
            {
                return BadRequest("Error Message: " + ex.ToString());
            }

            //object for DestinationCountryEntity 2
            DestinationCountryEntity SpecialFlightDeal2 = new DestinationCountryEntity("France", "Paris");
            SpecialFlightDeal2.Season = "Summer";
            SpecialFlightDeal2.PromoExpiry = new DateTime(2021, 11, 01);
            SpecialFlightDeal2.PromoCode = "VOYAGEPARIS11";

            try
            {
                //insert operation here
                TableOperation insert = TableOperation.Insert(SpecialFlightDeal1);
                TableResult result = table.ExecuteAsync(insert).Result;

                //get the result answer (success / not success to insert)
                ViewBag.TableName = table.Name;
                ViewBag.result = result.HttpStatusCode;
            }
            catch (Exception ex)
            {
                return BadRequest("Error Message: " + ex.ToString());
            }

            //object for DestinationCountryEntity 3
            DestinationCountryEntity SpecialFlightDeal3 = new DestinationCountryEntity("Mauritius", "Port Louis");
            SpecialFlightDeal3.Season = "Summer";
            SpecialFlightDeal3.PromoExpiry = new DateTime(2021, 10, 01);
            SpecialFlightDeal3.PromoCode = "FLYMRU33";

            try
            {
                //insert operation here
                TableOperation insert = TableOperation.Insert(SpecialFlightDeal1);
                TableResult result = table.ExecuteAsync(insert).Result;

                //get the result answer (success / not success to insert)
                ViewBag.TableName = table.Name;
                ViewBag.result = result.HttpStatusCode;
            }
            catch (Exception ex)
            {
                return BadRequest("Error Message: " + ex.ToString());
            }

            //object for DestinationCountryEntity 4
            DestinationCountryEntity SpecialFlightDeal4 = new DestinationCountryEntity("Japan", "Tokyo");
            SpecialFlightDeal4.Season = "Winter";
            SpecialFlightDeal4.PromoExpiry = new DateTime(2022, 02, 01);
            SpecialFlightDeal4.PromoCode = "FLYJPN44";

            try
            {
                //insert operation here
                TableOperation insert = TableOperation.Insert(SpecialFlightDeal1);
                TableResult result = table.ExecuteAsync(insert).Result;

                //get the result answer (success / not success to insert)
                ViewBag.TableName = table.Name;
                ViewBag.result = result.HttpStatusCode;
            }
            catch (Exception ex)
            {
                return BadRequest("Error Message: " + ex.ToString());
            }
            return View();

        }
        public ActionResult ViewSpecialFlightDeals()
        {
            string errormessage = null;
            CloudTable table = getStorageAccountInformation();
            try
            {
                TableQuery<DestinationCountryEntity> query = new TableQuery<DestinationCountryEntity>();
                List<DestinationCountryEntity> countries = new List<DestinationCountryEntity>();
                TableContinuationToken token = null; //to identify still have next data or not

                do
                {
                    TableQuerySegment<DestinationCountryEntity> result = table.ExecuteQuerySegmentedAsync(query, token).Result; //execute the query
                    token = result.ContinuationToken;

                    foreach (DestinationCountryEntity country in result.Results) // to retrieve every single data in one row
                    {
                        countries.Add(country);
                    }
                }
                while (token != null);

                if (countries.Count != 0)
                {
                    return View(countries); //back to display
                }
                else
                {
                    errormessage = "Data not found!";
                    return RedirectToAction("SearchPage", "Tables", new { dialogmsg = errormessage });
                }

            }
            catch (Exception ex)
            {
                errormessage = "Technical Issue: " + ex.ToString(); // technical error
            }
            return RedirectToAction("SearchPage", "Tables", new { dialogmsg = errormessage });
        }

        public ActionResult SearchPage(string dialogmsg = null)
        {
            ViewBag.msg = dialogmsg;
            return View();
        }

        //get the single data from the table storage
        public ActionResult getSingleEntity(string PartitionName, string RowName)
        {
            string errormessage = "";
            CloudTable table = getStorageAccountInformation();
            try
            {
                TableOperation retrieveprocess = TableOperation.Retrieve<DestinationCountryEntity>(PartitionName, RowName);
                TableResult result = table.ExecuteAsync(retrieveprocess).Result;
                if (result.Etag != null)
                {
                    return View(result); //if got result, bring the data to a new page called getsinglentity page
                }
                else
                {
                    errormessage = "Data not found in the table!"; //data not found answer
                }
            }

            catch (Exception ex)
            {
                errormessage = "Technical issue: " + ex.ToString();
            }
            return RedirectToAction("SearchPage", "Tables", new { dialogmsg = errormessage });
        }

        public ActionResult getGroupEntity(string PartitionName)
        {
            string errormessage = null;
            CloudTable table = getStorageAccountInformation();
            try
            {
                TableQuery<DestinationCountryEntity> query = new TableQuery<DestinationCountryEntity>()
                            .Where(TableQuery.GenerateFilterCondition("PartitionKey",
                             QueryComparisons.Equal, PartitionName));
                List<DestinationCountryEntity> countries = new List<DestinationCountryEntity>();
                TableContinuationToken token = null; //to identify still have next data or not

                do
                {
                    TableQuerySegment<DestinationCountryEntity> result = table.ExecuteQuerySegmentedAsync(query, token).Result; //execute the query
                    token = result.ContinuationToken;

                    foreach (DestinationCountryEntity country in result.Results) // to retrieve every single data in one row
                    {
                        countries.Add(country);
                    }
                }
                while (token != null);

                if (countries.Count != 0)
                {
                    return View(countries); //back to display
                }
                else
                {
                    errormessage = "Data not found!";
                    return RedirectToAction("SearchPage", "Tables", new { dialogmsg = errormessage });
                }

            }
            catch (Exception ex)
            {
                errormessage = "Technical Issue: " + ex.ToString(); // technical error
            }
            return RedirectToAction("SearchPage", "Tables", new { dialogmsg = errormessage });
        }
    }
}
