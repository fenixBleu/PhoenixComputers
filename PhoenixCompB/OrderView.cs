using System;
using System.Globalization;

namespace PhoenixComp
{
    class OrderView
    {
        //want to access these from main program, will set the access rights
        public enum compType{ Desktop, Laptop };
        
        public decimal total = 0.0m;
        public OrderView()
        {

            Console.WriteLine("Recent customized computer orders include: \n");
            // will need methods for reading the type of components and cost 
            //for now, we'll hardcode the date

            //computer 1
           

        }

        public void Computer(String type, decimal cost)
        {
            try
            {
                //base method
                //String cmpType;
                total = 0m;  //right now, we will  clear the total when calling this method
                total += cost;
                Console.WriteLine("\t {0}   {1, 45}", type, cost.ToString("C", CultureInfo.CurrentCulture));

            } catch (InvalidCastException ex)
            {

                Console.WriteLine(ex.Message);

            }
            catch (NullReferenceException ex)
            {

                Console.WriteLine("Null Reference Exception Occured: {0}", ex.Message);
            }



        }
        public void Computer(String type, decimal cost, Program.componentHW hardware, int size)
        {
            try
            {
                // this method provides the listing of hardware components, just going to use a base upgrade
                //size is currently memory size including GPU memory, will expand for determining parameters for components
                String descriptor;
                switch (hardware)
                {

                    case Program.componentHW.CPU:

                        descriptor = " GHz";

                        break;

                    default:

                        descriptor = " GB";
                        break;
                }
                total += cost;
                String Size = size.ToString() + descriptor;
                Console.WriteLine("\t     Upgrade {0} {1}:   {2}", hardware.ToString(), Size, cost.ToString("C", CultureInfo.CurrentCulture));

            }
            catch (ArithmeticException ex)
            {
                Console.WriteLine("Arithmetic Exception Occured: {0}", ex.Message);

            } 
            catch (InvalidCastException ex)
            {

                Console.WriteLine("Invalid Cast Exception Occured: {0}", ex.Message);

            }
            catch (NullReferenceException ex)
            {

                Console.WriteLine("Null Reference Exception Occured: {0}", ex.Message);
            }


        }
        public void Computer(String type, decimal cost, Program.componentSW software)
        {
            try
            {
                // this method provides the listing of software components
                // will look into expanding this depending upon the course
                total += cost;
                Console.WriteLine("\t     Install {0} {2}:  {1}", software.ToString(), cost.ToString("C", CultureInfo.CurrentCulture),
                    type);

            } catch (ArithmeticException ex)
            {
                Console.WriteLine("Arithmetic Exception Occured: {0}", ex.Message);
                
            }
            catch (InvalidCastException ex)
            {

                Console.WriteLine("Invalid Cast Exception Occured: {0}", ex.Message);

            }
            catch (NullReferenceException ex)
            {

                Console.WriteLine("Null Reference Exception Occured: {0}", ex.Message);
            }

        }


    }
}
