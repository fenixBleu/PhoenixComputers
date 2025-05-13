using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhoenixWebCore.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace PhoenixWebCore.Controllers
{
    public class BusinessController : Controller
    {
        private IWebHostEnvironment Environment;
        private readonly ILogger<BusinessController> _logger;

        public BusinessController(ILogger<BusinessController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            Environment = webHostEnvironment;

        }
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public IActionResult Products()
        {
            DataTable dt = new DataTable();
            string[] myCategories = { "Name", "Description", "Brand", "Price" };

            ProductModel products = new ProductModel();

            SQLiteCommand cmd;

            SQLiteDataAdapter ad;
            string location = "Data Source=" + string.Concat(this.Environment.ContentRootPath, "\\..\\database\\store_product.db");
            SQLiteConnection sQlite = new SQLiteConnection(location);
            string query = "select * from products";

            if (sQlite.State != ConnectionState.Open)
            {
                sQlite.Open();

            }

            cmd = sQlite.CreateCommand();

            cmd.CommandText = string.Format(query);

            ad = new SQLiteDataAdapter(cmd);

            //just filling a data table to format into an html table
            ad.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Component product = new Component();

                product.Name = Convert.ToString(dt.Rows[i][0]);
                product.Description = Convert.ToString(dt.Rows[i][1]);
                product.Brand = Convert.ToString(dt.Rows[i][2]);
                product.Price = Convert.ToDouble(dt.Rows[i][3]);
                product.Type = Convert.ToString(dt.Rows[i][4]);

                switch (product.Type)
                {

                    case "Laptop":

                        products.laptop.Add(product);
                        break;

                    case "Tablet":

                        products.tablet.Add(product);
                        break;

                    default:

                        products.accessory.Add(product);
                        break;
                }
            }

            return View(products);
        }
        [Authorize]
        public IActionResult Quotes()
        {
            //Fetch all files in the Folder (Directory).
            string[] filePaths = Directory.GetFiles(Path.Combine(this.Environment.ContentRootPath, "../Quotes/"));

            //Copy File names to Model collection.
            List<FileModel> files = new List<FileModel>();
            foreach (string filePath in filePaths)
            {
                files.Add(new FileModel { FileName = Path.GetFileName(filePath) });
            }

            return View(files);
        }

        public FileResult DownloadFile(string fileName)
        {
            //Build the File Path.
            string path = Path.Combine(this.Environment.ContentRootPath, "../Quotes/") + fileName;

            //Read the File data into Byte Array.
            //will be modifying this to enable larger files without taking large amounts of memory
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
        }

        [Authorize]
        public IActionResult Mailings()
        {
            MailingsModel mailing = new MailingsModel();
            string[] myCategories = { "Name", "Address", "E-mail", "Phone", "Category" };

            string[] files = Directory.GetFiles(string.Concat(this.Environment.ContentRootPath, "\\..\\mailinglist"));

            foreach (string file in files)
            {
                XDocument doc = XDocument.Load(file);

                var customers = from customer in doc.Descendants("Customers")
                                select new
                                {  //this is a modifed from an example I found.

                                    Children = customer.Descendants("Customer")
                                };

                foreach (var node in customers)
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
                            Mailing mail = new Mailing();

                            mail.Name = elements[0];
                            mail.Address = elements[1];
                            mail.Email = elements[2];
                            mail.Phone = elements[3];
                            mail.category = elements[4];

                            switch (mail.category)
                            {

                                case "Laptop":

                                    mailing.Laptop.Add(mail);
                                    break;

                                case "Desktop":

                                    mailing.Desktop.Add(mail);
                                    break;

                                case "Tablet":

                                    mailing.Tablet.Add(mail);
                                    break;

                                default:

                                    mailing.Accessory.Add(mail);
                                    break;
                            }
                        }
                        else
                        {
                            isEmpty = false;

                        }

                    }

                }

            }

            return View(mailing);
        }
        [Authorize]
        public IActionResult Users()
        {
            List<UserModel> userNames = new List<UserModel>();

            string[] files = Directory.GetFiles(string.Concat(this.Environment.ContentRootPath, "\\..\\users"));

            //we can later implement multiple user files without modifying this code
            foreach (string file in files)
            {
                XDocument doc = XDocument.Load(file);

                var user = from users in doc.Descendants("Users")
                           select new
                           {  //this is a modifed from an example I found.

                               Children = users.Descendants("User")
                           };
                foreach (var node in user)
                {
                    foreach (var name in node.Children)
                    {
                        UserModel userModel = new UserModel();
                        userModel.Name = getElementValue(name, "Name");
                        userNames.Add(userModel);


                    }

                }

            }




            return View(userNames);
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
