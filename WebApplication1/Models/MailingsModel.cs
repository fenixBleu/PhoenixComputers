using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixWebCore.Models
{
   
    public class Mailing
    {

        public string Name;
        public string Address;
        public string Email;
        public string Phone;
        public string category;

        public Mailing()
        {


        }


    }
    public class MailingsModel
    {

        public List<Mailing> Tablet = new List<Mailing>();
        public List<Mailing> Laptop = new List<Mailing>();
        public List<Mailing> Desktop = new List<Mailing>();
        public List<Mailing> Accessory = new List<Mailing>();

        public MailingsModel()
        {


        }

    }
}
