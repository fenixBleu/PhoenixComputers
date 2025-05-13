using System;
using System.Text.RegularExpressions;
using System.IO;


namespace PhoenixComp
{
    [Serializable]
    public class Customer
    {
        private string _name;
        private string _address;
        private string _email;
        private string _phone;
        private string _category;
        private string _street;
        private string _citystatezip;
        public string name { get { return _name; } set { _name = value; } }
        public string address { get { return _address; } set { _address = value; } }
        public string email { get { return _email; } set { _email = value; } }
        public string phone { get { return _phone; } set { _phone = value; } }
        public string street { get { return _street; }set { _street = value; } }
        public string citystatezip { get { return _citystatezip; } set { _citystatezip = value; } }
        public string category
        {
            get { return _category; }
            set { _category = value; }
        }
        

        public Customer()
        {

        }
        public Customer(string name, string address, string email, string phone)
        {

            this._name = name;
            this._address = address;
            this._email = email;
            this._phone = phone;

        }
        public void fillData()
        {
            try
            {
                //make this part of the customer class to prevent repeated code.
                Console.WriteLine("Customer's Name: ");
                name = Console.ReadLine();

                Address addr = new Address();

                Console.WriteLine("Enter Customer's Street: ");
                addr.street = Console.ReadLine();
                Console.WriteLine("Customer's City: ");
                addr.city = Console.ReadLine();
                Console.WriteLine("Customer's State: ");
                addr.state = Console.ReadLine();

                bool validZip = false;
                while (!validZip)
                {
                    //borrowed regex code, had to remove hidden chars. https://stackoverflow.com/questions/2577236/regex-for-zip-code
                    string _regex = @"^\d{5}(?:[-\s]\d{4})?$";
                    Console.WriteLine("Customer's Zip: ");
                    addr.zip = Console.ReadLine();
                    validZip = Regex.IsMatch(addr.zip, _regex);
                    if (!validZip)
                    {
                        Console.WriteLine("Please enter a zip code in proper format (xxxxx-xxxx) where last five characters are optional.");

                    }
                }
                
                citystatezip = addr.getCityStateZip();
                address = addr.getAddress();

                bool validEmail = false;
                while (!validEmail)
                {
                    //borrowed regex string from https://stackoverflow.com/questions/16167983/best-regular-expression-for-email-validation-in-c-sharp
                    string _regex = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
                    
                    Console.WriteLine("Customer's E-mail: ");
                    email = Console.ReadLine();

                    validEmail = Regex.IsMatch(email, _regex, RegexOptions.IgnoreCase);
                    if (!validEmail)
                    {
                        Console.WriteLine("An invalid e-mail format was entered. Please try again.");
                    }

                }
                

                bool validPhone = false;
                while (!validPhone)
                {
                    //found this regex online, had to make some corrections.
                    string _regex = @"^[\+]?[{1}]?[(]?[2-9]\d{2}[)]?[-\s\.]?[2-9]\d{2}[-\s\.]?[0-9]{4}$";

                    Console.WriteLine("Customer's Phone using format (xxx)xxx-xxxx: ");
                    phone = Console.ReadLine();
                    validPhone = Regex.IsMatch(phone, _regex);
                    if (!validPhone)
                    {
                        Console.WriteLine("Invalid phone number or invalid format. \n");
                    }


                }


            }
            catch(Exception ex)
            {
                if (ex is ArgumentException || ex is ArgumentNullException || ex is RegexMatchTimeoutException)
                {
                    Console.WriteLine("An error occured during the regex process: {0}", ex.HResult);
                }
                else if(ex is IOException)
                {
                    Console.WriteLine("An IO Exception has been thrown: {0}", ex.HResult);
                }
                else
                {
                    Console.WriteLine("An error has occured: {0}", ex.HResult);
                }
                //return null;

            }

        }



    }
}
