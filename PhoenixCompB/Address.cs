using System;
using System.Collections.Generic;
using System.Text;

namespace PhoenixComp
{
    class Address
    {

        private string _street;
        public string street { get { return _street; } set { _street = value; } }
        public string city;
        public string state;
        public string zip;

        public Address()
        {

        }
        public Address(string street, string city, string state, string zip)
        {

            this.street = street;
            this.city = city;
            this.state = state;
            this.zip = zip;


        }
        public string getCityStateZip()
        {

            return city + ", " + state + " " + zip;

        }

        public string getAddress()
        {

            return street + "\n" + city + ", " + state + " " + zip;

        }

    }
}
