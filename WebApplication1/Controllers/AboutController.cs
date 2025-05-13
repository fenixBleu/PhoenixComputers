using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhoenixWebCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PhoenixWebCore.Controllers
{
    public class AboutController : Controller
    {
        private IWebHostEnvironment Environment;
        private readonly ILogger<AboutController> _logger;

        public AboutController(ILogger<AboutController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            Environment = webHostEnvironment;

        }
        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        public IActionResult Locations()
        {
            string[] myCategories = { "storeid", "location", "address", "city", "state", "zip", "lat", "lon" };


            List<LocationsModel> locations = new List<LocationsModel>();

            //Load the XML file in XmlDocument.
            XDocument doc = XDocument.Load(string.Concat(this.Environment.ContentRootPath, "\\..\\database\\store_locations.txt"));

            var stores = from store in doc.Descendants("stores")
                         select new
                         {  //this is a modifed from an example I found.

                             Children = store.Descendants("store")
                         };

            //Loop through the selected Nodes.
            foreach (var node in stores)
            {
                foreach (var child in node.Children)
                {
                    bool isEmpty = false;

                    List<string> elements = new List<string>();
                    foreach (string element in myCategories)
                    {
                        string result = getElementValue(child, element);
                        if (string.IsNullOrEmpty(result))
                        {
                            isEmpty = true;


                        }
                        elements.Add(result);
                    }
                    if (!isEmpty)
                    {
                        LocationsModel location = new LocationsModel();

                        location.id = elements[0];
                        location.location = elements[1];
                        location.address = elements[2];
                        location.city = elements[3];
                        location.state = elements[4];
                        location.zip = elements[5];
                        location.latitude = elements[6];
                        location.longitude = elements[7];
                        locations.Add(location);

                    }
                    else
                    {
                        isEmpty = false;


                    }
                }

            }

            return View(locations);
            
        }
        protected string getElementValue(XElement xE, string elementName)
        {
            try
            {
                var data =
                    from item in xE.Descendants(elementName)
                    select item.Value;
                return data.ElementAt(0);
            }
            catch (Exception)
            {


                return null;

            }

        }
    }
}
