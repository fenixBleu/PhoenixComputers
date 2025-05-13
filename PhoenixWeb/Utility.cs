using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;
//using System.Windows;

namespace PhoenixWeb
{
    public class Utilities
    {
        protected DataTable dt;
        
        protected SQLiteCommand dbCommand;

        private static string location = "Data Source=" + Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        //Application.
        private static string dbName = "store_product.db";
        public static string FullPath = System.IO.Path.Combine(location, dbName);

        public static SQLiteConnection sQlite = new SQLiteConnection(FullPath);

        public static string query = "select name, description, brand, price from products where type='{0}'";
        

        //private static SQLiteDataAdapter ad;

       

       /* public  ListProducts( List<string> myCategories)
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
                        Label categoryName = new Label();
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

        } */



    }
}