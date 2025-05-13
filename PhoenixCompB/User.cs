using System;
using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
//using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

using System.Xml;
using System.Xml.Linq;
using System.Security.Cryptography;
//using System.Text;  

namespace PhoenixComp
{
    
    class User
    {
        
        private string _name;
       
        private string _fullPath;
        
        
        public string name {
            get{ return _name;
            } 
            set { _name = value; }
            }
        
        //not storing any hash or plain text pw here

        //not sure this should be a class as only one user at a time
        public User (string name,  string fullUserPath)
        {
            byte[] pw;
            _fullPath = fullUserPath;

            this.name = name;

            if (!File.Exists(_fullPath))
            {
                byte[] _salt;
                string _pw = GetPassword(true);
                _salt = CreateSalt(16);
                pw = EncryptPW(_salt, _pw);
                //we want to set the application to go ahead and get the password
                CreateFile(_fullPath, name, _salt, pw);

            }
            else
            {
                int i = 0;
                bool pwMatch = false;
                bool userMatch = false;
                //we want to search the file for the user first
                XDocument xDocument = XDocument.Load(_fullPath);
                XElement root = xDocument.Element("Users");
                IEnumerable<XElement> users = root.Descendants("User");
                foreach (XElement user in users)
                {

                    XElement element = user.Element("Name");
                    string val = element.Value;
                    if (val.Equals(name))
                    {
                        userMatch = true;
                        XElement _saltNode = user.Element("Salt");
                        XElement _pwNode = user.Element("Password");
                        byte[] _salt = Enumerable.Range(0, _saltNode.Value.Length / 2).Select(x => Convert.ToByte(_saltNode.Value.Substring(x * 2, 2), 16)).ToArray();
                        byte[] _pw = Enumerable.Range(0, _pwNode.Value.Length / 2).Select(x => Convert.ToByte(_pwNode.Value.Substring(x * 2, 2), 16)).ToArray();
                        while (i < 3 && !pwMatch)
                        {
                            
                            string pass = GetPassword(false);
                            byte[] tempPass = EncryptPW(_salt, pass);
                            if(_pw.Length == tempPass.Length)
                            {
                                for (int x = 0; x < _pw.Length; x++)
                                {
                                    if (_pw[x] != tempPass[x])
                                    {
                                        pwMatch = false;
                                        break;
                                    }
                                    pwMatch = true;
                                }
                            }
                            
                            if(!pwMatch && i < 3)
                            {
                                Console.WriteLine("Error logging in, please check credentials! \n");
                            }
                            i++;

                        }
                        if (!pwMatch)
                        {
                            Console.WriteLine("Your login attempts failed!");
                            Environment.Exit(1);
                        }

                    }
                  

                }
                if (!userMatch)
                {
                    //create a new record and password
                    Console.WriteLine("Credentials not found, please follow instructions to set password. \n");
                    byte[] _salt;
                    string _pw = GetPassword(true);
                    _salt = CreateSalt(16);
                    pw = EncryptPW(_salt, _pw);
                    AddUser(_fullPath, name, _salt, pw);

                }


            }
            
        }
        private byte[] CreateSalt(int saltSize)
        {
            //each user uses a different salt
            
            RandomNumberGenerator rng = RandomNumberGenerator.Create();

            byte[] salt = new byte[saltSize];
            rng.GetBytes(salt);

            return salt;
        }
       
        public byte[] EncryptPW(byte[] salt, string pw)
        {
            //as it says, returns a salted+hash password
            byte[] encrypted = KeyDerivation.Pbkdf2(pw, salt, KeyDerivationPrf.HMACSHA512, 101, 64);

            return encrypted;

        }
       
        private void CreateFile(string fullPath, string name, byte[] salt, byte[] pw)
        {
            // this method creates a file if it does not exist and adds the first user
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;
            xmlWriterSettings.NewLineOnAttributes = true;
            xmlWriterSettings.WriteEndDocumentOnClose = true;
            using (XmlWriter xmlWriter = XmlWriter.Create(fullPath, xmlWriterSettings))
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("Users");
                xmlWriter.WriteStartElement("User");
                xmlWriter.WriteElementString("Name", name);
                xmlWriter.WriteStartElement("Salt");
                xmlWriter.WriteBinHex(salt, 0, salt.Count());
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("Password");
                xmlWriter.WriteBinHex(pw, 0, pw.Count());
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
                xmlWriter.Flush();
                xmlWriter.Close();

            }
            //normally here, I would reset the username and have the user begin the log in process again.
        }
        private void AddUser(string fullPath, string name, byte[] salt, byte[] pw)
        {

            XDocument xDocument = XDocument.Load(fullPath);
            XElement root = xDocument.Element("Users");
            IEnumerable<XElement> rows = root.Descendants("User");
            XElement lastRow = rows.Last();

            //changed to add next record after
            string saltString = ToHexString(salt);
            string pwString = ToHexString(pw);
            lastRow.AddAfterSelf(
                  new XElement("User",
                  new XElement("Name", name),
                  new XElement("Salt", saltString),
                  new XElement("Password", pwString)));
            
            xDocument.Save(fullPath);
            

        }
        private string ToHexString(byte[] ba)
        {
            //modified from an example on stackoverflow.org
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();

        }
        
        private string ReadPassword()
        {
            //borrowed this code from: https://stackoverflow.com/questions/3404421/password-masking-console-application
            //and slightly modified it.
            string pass = "";
            Console.WriteLine("Enter your password: ");
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                // Backspace Should Not Work
                // B Vocque, should not add the enter key (\r) either
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    Console.Write("\b");
                }
            }
            // Stops Receving Keys Once Enter is Pressed
            while (key.Key != ConsoleKey.Enter);
            Console.WriteLine();
            return pass;

        }
        private string GetPassword(bool newPw)
        {
            string finalPW = "";
            try
            {

            
                if (newPw)
                {
                    bool incomplete = true;
                    Console.WriteLine("Passwords need to be a minimum of 8 characters, and at least one number, one uppercase letter, one lowercase, and \n " +
                        "one of the following special characters: !@#$%*.~");
                    while (incomplete)
                    {
                        bool noMatch = true;
                        string pwA = "";
                        //regex string to enforce password strength
                        string regex = "^(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z])[a-zA-Z0-9!@#$%*.~]{8,}$";
                        while (noMatch)
                        {
                            Console.WriteLine("Please Enter a Password:");
                            pwA = ReadPassword();
                            noMatch = !Regex.IsMatch(pwA, regex);

                        }
                        Console.WriteLine("Please Confirm Your Password: ");
                        string pwB = ReadPassword();
                        if (pwA.Equals(pwB))
                        {
                            finalPW = pwA;
                            incomplete = false;
                        } else
                        {
                            Console.WriteLine("Passwords did not match.  Please try again. Press enter to continue.");
                            Console.ReadLine();

                        }
                    }


                } else
                {
                    Console.WriteLine("Please enter your password to log in:\n");
                    finalPW = ReadPassword();

                }
                return finalPW;


            }
            catch (Exception ex)
            {
                if(ex is ArgumentException || ex is ArgumentNullException || ex is RegexMatchTimeoutException)
                {
                    Console.WriteLine("An error occured during the regex process: {0}", ex.HResult);
                }
                else
                {
                    Console.WriteLine("An error has occured: {0}", ex.HResult);
                }
                return null;

            }
        }


    }
}
