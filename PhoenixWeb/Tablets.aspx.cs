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
using MessageBox = System.Windows.MessageBox;

namespace PhoenixWeb
{
    public partial class Tablets : System.Web.UI.Page
    {
       
        protected SQLiteDataAdapter ad;
        private SQLiteCommand cmd;

        protected DataTable dt = new DataTable();
        private string type = "Tablet";

        private string[] myCategories = { "Name", "Description", "Brand", "Price" };
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
                result = (DialogResult)MessageBox.Show(message, "SQLite Exception", btn);

            }

            catch (NullReferenceException ex)
            {

                MessageBoxButton btn = MessageBoxButton.OK;
                string message = "Please inform admin the following error resulted: " + ex.Message;
                DialogResult result;
                result = (DialogResult)MessageBox.Show(message, "Null Reference Exception", btn);

            }

        }
        protected void ListProducts()
        {
            //will look into making this a global function for all pages

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

            for (int i = 0; i < dt.Rows.Count; i++)
            {


            }

        }
    }
}
