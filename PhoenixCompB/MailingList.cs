using System;
using System.Collections.Generic;
using System.IO;

using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace PhoenixComp
{
    class MailingList
    {
        protected string path = System.Environment.CurrentDirectory;//  Assembly.GetEntryAssembly().Location;
        protected string fullPath; //= path + "\\mailinglist\category.xml";
        protected string filePath;
       


        public MailingList()
        {
            Customer cust = new Customer();
            string pth = Directory.GetParent(path).Parent.Parent.FullName;
            fullPath = pth + "\\mailinglist\\";

            


        }
        
        public void writeRecord(Customer cust)
        {

            filePath = fullPath + cust.category + ".xml";

            //modified example. https://stackoverflow.com/questions/20922835/appending-an-existing-xml-file-with-xmlwriter
            if (!File.Exists(filePath))
            {
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                xmlWriterSettings.Indent = true;
                xmlWriterSettings.NewLineOnAttributes = true;
                xmlWriterSettings.WriteEndDocumentOnClose = true;
                using (XmlWriter xmlWriter = XmlWriter.Create(filePath, xmlWriterSettings))
                {
                    xmlWriter.WriteStartDocument();
                    xmlWriter.WriteStartElement("Customers");

                    xmlWriter.WriteStartElement("Customer");
                    xmlWriter.WriteElementString("Name", cust.name);
                    xmlWriter.WriteElementString("Address", cust.address);
                    xmlWriter.WriteElementString("E-mail", cust.email);
                    xmlWriter.WriteElementString("Phone", cust.phone);
                    xmlWriter.WriteElementString("Category", cust.category);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndDocument();
                    xmlWriter.Flush();
                    xmlWriter.Close();
                }
            }
            else
            {
                XDocument xDocument = XDocument.Load(filePath);
                XElement root = xDocument.Element("Customers");
                IEnumerable<XElement> rows = root.Descendants("Customer");
                XElement lastRow = rows.Last();
                
                //changed to add next record after
                lastRow.AddAfterSelf(
                      new XElement("Customer",
                      new XElement("Name", cust.name),
                      new XElement("Address", cust.address),
                      new XElement("E-mail", cust.email),
                      new XElement("Phone", cust.phone),
                      new XElement("Category", cust.category)));
                
                xDocument.Save(filePath);

            }
            


        }

        public void Close()
        {
            

        }
    }
}
