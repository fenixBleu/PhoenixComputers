using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using PhoenixWebCore.Models;
using System.Text.Encodings.Web;
using System.Xml;
using System.Xml.Linq;


//using DotnetcoreDynamicJSONRPC;

namespace PhoenixWebCore.Controllers
{
    public class HomeController : Controller
    {
       
        private IWebHostEnvironment Environment;

        
        //public HomeController(IWebHostEnvironment _environment)
        //{
            //this.Environment = _environment;
        //}
        private readonly ILogger<HomeController> _logger;
        public string Welcome(string name, int numTimes = 1)
        {
            return HtmlEncoder.Default.Encode($" {name}");
        }
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            Environment = webHostEnvironment;
            
        }
        [Authorize]
        public IActionResult Index()
        {
            string body = "Please select the desired tab from the above menu.";
            string name = User.Identity.Name;
            if ((name == "" )|| (name == null)) 
            {
                name = " Visitor";
                body = "Please log in to continue.";
            }
            string path = Environment.ContentRootPath;

            ViewData["Message"] = Welcome(name);
            ViewData["Body"] = HtmlEncoder.Default.Encode($" {body} ");
            
            
            return View();
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
