using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace PhoenixWeb
{
    public partial class Locations : System.Web.UI.Page
    {
        //implement a method to read the element names and populate this
        protected string[] myCategories = { "storeid", "location", "address", "city", "state","zip", "lat", "lon" };
        protected string defaultSearch = "store";
        protected string root = "stores";

        //want to reuse fullPath throughout the class
        string fullPath; 

        //accessible throughout the clss
        protected HtmlTable table = new HtmlTable();

        protected void Page_Load(object sender, EventArgs e)
        {

            //StringBuilder result = new StringBuilder();

            //will use this for the sql database as well
            string path = this.Server.MapPath("");
            fullPath = path.Insert(path.Length, "\\database\\store_locations.txt");


            //check to see if the table is already populated and clear if if so
            if (placeholder.Controls.Count > 0)
            {
                placeholder.Controls.Clear();
            }
            try
            {

                //load the full file into the xml document object
                XDocument xdoc = XDocument.Load(fullPath);

                var stores = from store in xdoc.Descendants(root)
                             select new
                             {  //this is a modifed from an example I found.

                                 Children = store.Descendants(defaultSearch)
                             };
                //add the header
                placeholder.Controls.Add(createHeader());

                foreach (var node in stores)
                {
                    foreach (var child in node.Children)
                    {
                        bool isEmpty = false;
                        HtmlGenericControl tr = new HtmlGenericControl("tr");
                        List<string> elements = new List<string>();

                        foreach (string element in myCategories)
                        {
                            string result = getElementValue(child, element);
                            if (string.IsNullOrEmpty(result))
                            {
                                isEmpty = true;


                            }
                            elements.Add(result);
                            if (element.Equals("city"))
                            {
                                ListItem listItem = DropDownList1.Items.FindByValue(result);
                                if (listItem == null)
                                {

                                    DropDownList1.Items.Add(result);

                                }



                            }
                        }
                        if (!isEmpty)
                        {
                            tr = addToRow(tr, createCell(elements[0], "center"));
                            tr = addToRow(tr, createCell(elements[1], "center"));

                            string address = formatAddress(elements[2], elements[3], elements[4], elements[5]);
                            tr = addToRow(tr, createCell(address, "left"));

                            tr = addToRow(tr, createCell(elements[6], "center"));
                            tr = addToRow(tr, createCell(elements[7], "center"));

                            placeholder.Controls.Add(tr);


                        }
                        else
                        {
                            isEmpty = false;
                            //break;

                        }


                    }

                }
            }
            catch (NullReferenceException ex)
            {

                MessageBoxButton btn = MessageBoxButton.OK;
                string message = "Please inform admin the following error resulted: " + ex.Message;
                DialogResult result;
                result = (DialogResult)System.Windows.MessageBox.Show(message, "Null Reference Exception", btn);

            }
            catch (FormatException ex)
            {

                MessageBoxButton btn = MessageBoxButton.OK;
                string message = "Error, failed to convert value: " + ex.Message.ToString();
                DialogResult result;
                result = (DialogResult)System.Windows.MessageBox.Show(message, "Conversion Exception", btn);


            }
            catch (Exception ex)
            {
                MessageBoxButton btn = MessageBoxButton.OK;
                string message = "Please inform admin the following error resulted: " + ex.Message;
                DialogResult result;
                result = (DialogResult)System.Windows.MessageBox.Show(message, "Other Exception", btn);

            }
                      
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
            catch (Exception ex)
            {
                MessageBoxButton btn = MessageBoxButton.OK;
                string message = "Please inform admin the following error resulted: " + ex.Message;
                DialogResult result;
                result = (DialogResult)System.Windows.MessageBox.Show(message, "Other Exception", btn);

                return null;

            }

        }
         
        //This function takes an XML Document, name of element to search, and the element value(s) we are looking for and
        //returns the data found, if any.
        protected IEnumerable<XElement> SearchXML(XDocument xdoc, string searchElement, string parameter)
        {
            try
            {

                IEnumerable<XElement> xE = xdoc.Descendants(defaultSearch).Where(x => x.Element(searchElement).Value == parameter);

                return xE;
            }
            catch (NullReferenceException ex)
            {

                MessageBoxButton btn = MessageBoxButton.OK;
                string message = "Please inform admin the following error resulted: " + ex.Message;
                DialogResult result;
                result = (DialogResult)System.Windows.MessageBox.Show(message, "Null Reference Exception", btn);

                return null;

            }

        }

        //create cells, should be able to use this as a set of overriden global methods.
        protected HtmlGenericControl createCell(string data, string align)
        {
            HtmlGenericControl c = new HtmlGenericControl("td");
            c.Controls.Add(new LiteralControl(data));
            c.Attributes.Add("height", "75px");
            c.Attributes.Add("text-align", align);

            return c;

        }

        //pass the parameters as strings, format for display in htlm cell
        protected string formatAddress(string address, string city, string state, string zip)
        {
            string lineBreak = "<br>";
            string s = address + lineBreak + city + ", " + state + " " + zip + lineBreak;
            return s;

        }

        protected HtmlGenericControl addToRow(HtmlGenericControl tr, HtmlGenericControl td)
        {
            tr.Controls.Add(td);
            return tr;

        }

        //build the category labels
        protected DataTable Categories(string[] categories)
        {


            DataTable dt = new DataTable();
            dt = new DataTable();

            for (int i = 0; i < categories.Length; i++)
            {
                dt.Columns.Add(categories[i], typeof(String));


            }
            //when needing to test
            //dt.Rows.Add(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

            return dt;

        }
        protected HtmlGenericControl createHeader()
        {
            string[] columns = { "ID", "Location", "Address", "Lat", "Lon" };
            HtmlGenericControl tr = new HtmlGenericControl("tr");
            for (int i = 0; i < columns.Length; i++)
            {
                
                

                        HtmlGenericControl th = new HtmlGenericControl("th");
                        String fieldName = Convert.ToString(columns[i]);
                        System.Web.UI.WebControls.Label categoryName = 
                            new System.Web.UI.WebControls.Label();
                        categoryName.ID = "lb" + fieldName;
                        categoryName.Text = columns[i];

                        th.Controls.Add(categoryName);
                        th.Attributes.Add("width", "225px");
                        th.Attributes.Add("text-align", "center");
                        tr.Controls.Add(th);
                        tr.Attributes.Add("height", "50px");
                        
                        

             }
            return tr;


         }

        protected void Submit_Click(object sender, EventArgs e)
        {
            if(placeholder.Controls.Count > 0)
            {

                placeholder.Controls.Clear();
                placeholder.Controls.Add(createHeader());


            }

            XDocument xdoc = XDocument.Load(fullPath);
            string selection = DropDownList1.Text;

            var stores =  from store in xdoc.Descendants(root)
                         select new
                         {
                             
                             Children = store.Descendants(defaultSearch).Where(x => x.Element("city").Value == selection)
                         };

            bool isEmpty = false;
            HtmlGenericControl tr = new HtmlGenericControl("tr");
            List<string> elements = new List<string>();

            Reset.Enabled = true;
            foreach (var nodes in stores)
            {
                foreach(var child in nodes.Children)
                {

                    foreach (string element in myCategories)
                    {
                        string result = getElementValue(child, element);
                        if (string.IsNullOrEmpty(result))
                        {
                            isEmpty = true;


                        }
                        else
                        {
                            elements.Add(result);

                        }

                    }
                    if (!isEmpty)
                    {
                        tr = addToRow(tr, createCell(elements[0], "center"));
                        tr = addToRow(tr, createCell(elements[1], "center"));

                        string address = formatAddress(elements[2], elements[3], elements[4], elements[5]);
                        tr = addToRow(tr, createCell(address, "left"));

                        tr = addToRow(tr, createCell(elements[6], "center"));
                        tr = addToRow(tr, createCell(elements[7], "center"));

                        placeholder.Controls.Add(tr);

                    }
                }


            }
        }

        protected void Reset_Click(object sender, EventArgs e)
        {
            Reset.Enabled = false;
            //this refreshes the page
            Response.Redirect(Request.Path);
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //was experimenting,
        }
    }
    
}