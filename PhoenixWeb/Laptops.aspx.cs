using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Forms;

namespace PhoenixWeb
{
    public partial class Laptops : System.Web.UI.Page 
   
    {
        //looking into applying the configuration data to the web server....
        protected SQLiteConnection sQlite;
        protected SQLiteCommand cmd;

        
        protected SQLiteDataAdapter ad;

        protected DataTable dt = new DataTable();

        string[] myCategories = { "Name", "Description", "Brand", "Price"};
        private string type = "Laptop";

        protected void Page_Load(object sender, EventArgs e)
        {
            
            //let's retrieve the data using an adapter
            //
            try
            {
               

                

                if (Utilities.sQlite.State != ConnectionState.Open)
                {
                    Utilities.sQlite.Open();

                }
                
                
                cmd = Utilities.sQlite.CreateCommand();
                
                cmd.CommandText = string.Format(Utilities.query, type);

                ad = new SQLiteDataAdapter(cmd);

                //just filling a data table to format into an html table
                ad.Fill(dt);

                ListProducts();

               

            }
            catch (SQLiteException ex)
            {
                MessageBoxButton btn = MessageBoxButton.OK;
                string message = "Please inform admin the following SQLite error resulted: " + ex.ResultCode.ToString();
                DialogResult result;
                result = (DialogResult)System.Windows.MessageBox.Show(message, "SQLite Exception", btn);
                //will add some code here to handle these exceptions

            }
            

        }

        protected void ListProducts()
        {
            try
            {
                //will look into making this a global function for all pages
                //need to learn how to call/pass the asp paramters...
                FormatException eEx = new FormatException();
                
                  
                for (Int32 i = 0; i < dt.Rows.Count; i++)
                {
                    
                    HtmlGenericControl tr = new HtmlGenericControl("tr");
                    TableRow r = new TableRow();

                    if (i == 0)
                    {
                        for (int x = 0; x < dt.Columns.Count; x++)
                        {
                            HtmlGenericControl th = new HtmlGenericControl("th");
                            String fieldName = Convert.ToString(dt.Rows[i][myCategories[x]]);
                            System.Web.UI.WebControls.Label categoryName = new System.Web.UI.WebControls.Label();
                            categoryName.ID = "lb" + fieldName;
                            categoryName.Text = myCategories[x];

                            th.Controls.Add(categoryName);
                            th.Attributes.Add("width", "225px");
                            th.Attributes.Add("text-align", "center");
                            tr.Controls.Add(th);
                            tr.Attributes.Add("height", "50px");
                            placeholder.Controls.Add(tr);

                            
                        }


                    }
                    else
                    {


                        for (int x = 0; x < dt.Columns.Count; x++)
                        {
                            //will go with this one for creating tables/celll
                            String fieldData = Convert.ToString(dt.Rows[i - 1][x]);

                            TableCell c = new TableCell();
                            c.Controls.Add(new LiteralControl(fieldData));
                            c.Attributes.Add("height", "25px");
                            r.Controls.Add(c);


                            placeholder.Controls.Add(r);
                        }


                    }

                }

            } catch (FormatException ex)
            {

                MessageBoxButton btn = MessageBoxButton.OK;
                string message = "Error, failed to convert value: " + ex.Message.ToString();
                DialogResult result;
                result = (DialogResult)System.Windows.MessageBox.Show(message, "Conversion Exception", btn);


            }

        }
    }
}