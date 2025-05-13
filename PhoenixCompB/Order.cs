using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;




namespace PhoenixComp
{
    class Order
    {
        private Customer customer;
        
        private List<Components> components = new List<Components>();
        private double total = 0.0;
        protected string path = System.Environment.CurrentDirectory;
        private string basePath;
        private string fullPath;
        //Application excel = new Application();
        Application excel = new Microsoft.Office.Interop.Excel.Application();

        public Order(Customer cust)
        {
            string datetime;
            DateTime dt = DateTime.Now;
            datetime = dt.ToString("MMddyyHHmm");
            //dt.GetDateTimeFormats()
            customer = cust;
            basePath = Directory.GetParent(path).Parent.Parent.Parent.FullName + "\\quotes";
            fullPath = basePath + "\\" + customer.name + datetime + ".xlsx";

        }

        
        public void orderComponents()
        {
            Components component = new Components();

            //ensure we get a value
            while (component.name == null)
            {
                Console.WriteLine("Enter Component: ");
                component.name = Console.ReadLine();

            }
            
            Console.WriteLine("Enter Component Cost:");

            //ensure we get a value and it can convert to double
            
            bool isDouble = false;
            while (!isDouble)
            {
                string cost = Console.ReadLine();
                if (cost != "")
                {
                    isDouble = double.TryParse(cost, out double itemCost);
                    if (isDouble)
                    {
                        component.cost = Math.Round(itemCost, 2);
                    }

                }
                

            }
            
            total += component.cost;
            components.Add(component);



        }
        public List<Components> getList()
        {

            return components;

        }
        public void PostOrder()
        {
            object misValue = System.Reflection.Missing.Value;

            try
            {


                Workbook xlWB = excel.Workbooks.Add(misValue);

                Worksheet xlWS = (Worksheet)xlWB.Worksheets.get_Item(1);
                xlWS.Name = customer.name;

                Microsoft.Office.Interop.Excel.Range headerRange = excel.get_Range("B1:D1", Type.Missing);

                headerRange.MergeCells = true;
                headerRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                headerRange.Font.Bold = true;

                headerRange.Value = "Phoenix Computers Sales Order";

                xlWS.Cells[2, 1] = "Customer:";
                xlWS.Cells[2, 1].Font.Bold = true;

                xlWS.Cells[2, 2] = customer.name;
                xlWS.Cells[3, 2] = customer.street;
                xlWS.Cells[4, 2] = customer.citystatezip;
                xlWS.Cells[5, 2] = customer.email;
                xlWS.Cells[6, 2] = customer.phone;

                xlWS.Cells[9, 2] = "Item #";
                xlWS.Cells[9, 2].Font.Bold = true;
                xlWS.Cells[9, 2].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                xlWS.Cells[9, 3] = "Description";
                xlWS.Columns[3].ColumnWidth = 15;
                xlWS.Cells[9, 3].Font.Bold = true;
                xlWS.Cells[9, 3].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                xlWS.Cells[9, 4] = "Price";
                xlWS.Cells[9, 4].Font.Bold = true;
                xlWS.Cells[9, 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                int i = 10;
                int x = 1;
                foreach (Components comp in components)
                {

                    xlWS.Cells[i, 2] = x;
                    xlWS.Cells[i, 3] = comp.name;
                    xlWS.Cells[i, 4] = comp.cost;
                    i++;
                    x++;

                }
                xlWS.Cells[i, 3] = "Total:";
                xlWS.Cells[i, 3].Font.Bold = true;
                xlWS.Cells[i, 3].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                xlWS.Cells[i, 4] = total;
              

                xlWB.SaveAs(fullPath, Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook,
                    misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive,
                    misValue, misValue, misValue, misValue, misValue);


                xlWB.Close(true, misValue, misValue);

                Console.WriteLine("Quote was saved to: {0}", fullPath);

                Marshal.ReleaseComObject(xlWS);
                Marshal.ReleaseComObject(xlWB);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error experienced while posting order: {0}", ex.Message);

            }

        }


    }
}
