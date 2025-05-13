using System;
using System.Globalization;
using System.IO;

/* 
 
Course:  ICT4351
Author:  Bobby Vocque

*/

namespace PhoenixComp
{
    class Program
    {
        public enum componentHW { Memory, CPU, GPU };
        public enum componentSW { OS, SW };
        public static MailingList ml;
        protected static string fullUserPath;
        static void Main(string[] args)
        {
            //local declarations, keep up front to ease modifications
            String storeName = "Phoenix Computers";
            String ownerLabel = "Owner: ";
            String ownerName = "Bobby Vocque";
            String readDesc = "Please enter your name.";
            String userName;
            String output;
            String input = "";
            
            string path = System.Environment.CurrentDirectory + "\\..\\..\\..\\users\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            fullUserPath = path + "users.xml";
            

            
            Console.WriteLine(storeName);
            Console.WriteLine(ownerLabel + ownerName + "\n");
            //Console.WriteLine();
            Console.WriteLine(readDesc);
            userName = Console.ReadLine();
            User user = new User(userName, fullUserPath);
            Console.WriteLine();

            DateTime dateTime = DateTime.Now.ToLocalTime();  //using local OS regional settings

            //play with this more to determine if this can be moved entirely to the local var defs
            output = String.Format("Welcome {0}, I hope you are having a good day. {1} \n", userName, 
                dateTime.ToString("dddd, dd MMMM yyyy HH:mm")); 
            
            Console.WriteLine(output);

            //we want the loop to continue until the exit condition is met.  Currently, any extry other than "1"
            bool continueLoop = true;
            Customer cust = new Customer();
            while (continueLoop)
            {

                Console.WriteLine("Press 1 to view customized orders or press any other key to exit the application. \n" +
                    "Press 2 to add a customer to the mailing list. \nPress 3 to add a customer order.");
                input = Console.ReadLine();
                
                switch (input) {

                    case "1":
                        //will do this for now until I know/decide how the orders will be stored.
                        OrderView view = new OrderView();
                        view.Computer("Desktop", 500m);
                        view.Computer("GPU", 250m, Program.componentHW.GPU, 6);
                        view.Computer("Windows 11", 125m, componentSW.OS);
                        Console.WriteLine("\t Total is: {0,45} \n\n", view.total.ToString("C", CultureInfo.CurrentCulture));

                        view.Computer("Laptop", 700m);
                        view.Computer("Memory", 50m, Program.componentHW.Memory, 16);
                        view.Computer("Office", 150m, componentSW.SW);
                        Console.WriteLine("\t Total is: {0,45} \n\n", view.total.ToString("C", CultureInfo.CurrentCulture));

                        view.Computer("Laptop", 650m);
                        view.Computer("CPU", 350m, Program.componentHW.CPU, 4);
                        view.Computer("Antivirus", 75m, componentSW.SW);
                        Console.WriteLine("\t Total is: {0,45} \n\n", view.total.ToString("C", CultureInfo.CurrentCulture));

                        break;

                    case "2":
                        
                        
                        ml = new MailingList();
                        
                        Console.WriteLine("Enter the following items for the mailing list.");

                        cust.fillData();

                        Console.WriteLine("Customer's Mailing List Category: ");
                        cust.category = Console.ReadLine();
                        
                        ml.writeRecord(cust);
                        ml.Close();
                        break;

                    case "3":

                        Console.WriteLine("Enter the following items for the customer order.");
                        cust.fillData();
                        Order order = new Order(cust);
                        bool continueOrder = true;
                        while (continueOrder)
                        {
                            Console.WriteLine("Would you like to add an item to the order? (y/n)");
                            string answer = Console.ReadLine().ToUpper();
                            if (answer.Equals("Y"))
                            {
                                Console.WriteLine("Enter descriptive information (model number, RAM/HDD size of the component.");
                                order.orderComponents();

                            } else
                            {
                                continueOrder = false;
                            }


                        }
                        order.PostOrder();
                        Console.WriteLine("Would you like to review the order?");
                        string review = Console.ReadLine();
                        if (review.ToUpper().Equals("Y"))
                        {
                            Console.WriteLine();
                            Console.WriteLine("Customer Name: {0}", cust.name);
                            Console.WriteLine("Street Adress: {0}", cust.street);
                            Console.WriteLine("City, State Zip: {0}", cust.citystatezip);
                            Console.WriteLine();
                            Console.WriteLine("Order items: ");
                            int i = 1;
                            foreach(Components comp in order.getList())
                            {
                                Console.WriteLine("Item {0}: {1}  Cost: {2}", i, comp.name, comp.cost);


                                i++;
                            }


                        }
                        continueLoop = false;
                        break;

                    default:

                        //if we reach this block, we are exiting.
                        continueLoop = false;
                        break;


                }


            }
            // will add error checking later and exit with the correct code
            
            
            Environment.Exit(0);
            

        }
        public static string getName()
        {
             

            Console.WriteLine("Customer's Name: ");
            string name = Console.ReadLine();
            return name;

        }
        public static string getAddress()
        {
            Address addr = new Address();

            Console.WriteLine("Enter Customer's Street: ");
            addr.street = Console.ReadLine();
            Console.WriteLine("Customer's City: ");
            addr.city = Console.ReadLine();
            Console.WriteLine("Customer's State: ");
            addr.state = Console.ReadLine();
            Console.WriteLine("Customer's Zip: ");
            addr.zip = Console.ReadLine();

            return addr.getAddress();

        }
        public static string getEmail()
        {

            Console.WriteLine("Customer's E-mail: ");
            return Console.ReadLine();


        }
    }
}
