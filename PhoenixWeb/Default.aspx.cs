using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data.SQLite;
using System.Globalization;

namespace PhoenixWeb
{
 

    public partial class _Default : Page
    {
        DataTable products = new DataTable();
        //DBServer dataBase;
        protected SQLiteConnection sQlite;
        protected SQLiteCommand dbCommand;

        private static string location = "Data Source=C:/Users/vocqu/source/repos/PhoenixComp/PhoenixWeb/";
        private static string dbName = "store_product.db";
        protected string fullPath = System.IO.Path.Combine(location, dbName);
        
        string[] myCategories = { "Desktops", "Laptops", "Cell Phones", "Tablets", "Acessories" };
        string[,] myComputers = { { "HP", "Lightning 700", "$800" }, { "HP", "Thunderbolt", "$950" },
        {"Dell","Warp Drive","$1000" },{"Dell", "TransWarp","$1175" },{ "Gateway", "Lightspeed", "$975"} };
        List<Tuple<string, string>> tablets = new List<Tuple<string, string>>();

        protected void Page_Load(object sender, EventArgs e)
        {
            //we are going to add some tablets, working with tuples on an real project and just want to exercise
            //will probably convert this to a hashmap or some other collection
            Tuple<string,string> tab = new Tuple<string, string>("Apple", "Big Bite $1200");
            Tuple<string, string> tabB = new Tuple<string, string>("LG", "Enjoy $225");
            tablets.Add(tab);
            
            tablets.Add(tabB);

            sQlite = new SQLiteConnection(fullPath);
            sQlite.Open();
            var success = sQlite.State;


            CreateTable();

            

        }

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
        protected DataTable CreateComputers(DataTable dt, string[,] computers)
        {
            for (int i = 0; i < computers.GetLength(0); i++)
            {
                string current = computers[i, 0] + " " + computers[i, 1] + " " + computers[i, 2];
                
                if (dt.Rows.Count < i + 1)
                {   
                    //add the computer product, set other fields from null to empty
                    dt.Rows.Add(current, string.Empty, string.Empty, string.Empty,string.Empty);
                    string query = string.Format("Select count(*) from products WHERE Exists(select * from products where type='{0}' and brand='{1}' and name='{2}')",
                        "Computer", computers[i, 0], computers[i, 1]);

                    //will come back to this.
                    /*int amount = int.Parse(computers[i, 2], NumberStyles.Currency);
                    dbCommand = new SQLiteCommand(query, sQlite);
                    var existed = dbCommand.ExecuteScalar();
                    if(existed.ToString() == "0")
                    {
                        query = string.Format("insert into products ('name','description', 'brand', 'price', 'type') values ('{0}','{1}','{1}',{2},'Computer')", computers[i, 0], computers[i, 1], amount);
                        dbCommand.CommandText = query;
                        dbCommand.ExecuteReader();



                    }*/

                }
                else
                {
                    //just need to populate the first column
                    dt.Rows[i].SetField(0, current);
                    

                }

            }
            return dt;

        }
        protected DataTable CreateTablets(DataTable dt, List<Tuple<string,string>> tablets)
        {   
            if (tablets.Count > 0)
            {

                int i = 0;
                foreach ( Tuple<string,string> tablet in tablets)
                {
                    string tab = tablet.Item1 + " " + tablet.Item2;
                    dt.Rows[i].SetField(3, tab);
                    i++;
                }

            }

            return dt;
        }
        protected void CreateTable()
        {
            //using a DataTable to sort the data, could bind this to a GridView
            DataTable dt = new DataTable();
            dt = Categories(myCategories);
            dt = CreateComputers(dt, myComputers);
            dt = CreateTablets(dt, tablets);

           

            if (dt.Columns.Count > 0)
            {
                
                
                
                //HtmlGenericControl td1 = new HtmlGenericControl("td");

                //experimented with a couple of ways generating table data

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
                        

                    } else
                    {

                        
                        for (int x = 0; x < dt.Columns.Count; x++)
                        {
                            //will go with this one for creating tables/celll
                            String fieldData = Convert.ToString(dt.Rows[i-1][x]);

                            TableCell c = new TableCell();
                            c.Controls.Add(new LiteralControl(fieldData));
                            c.Attributes.Add("height", "25px");
                            r.Controls.Add(c);

                            
                            placeholder.Controls.Add(r);
                        }
                        

                    }
                    
                }

            }

        }
        
    }
}