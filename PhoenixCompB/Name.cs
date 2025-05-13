

namespace PhoenixComp
{
    class Name
    {
        protected string fName;
        protected string lName;

        public Name(string fName, string lName)
        {
            this.fName = fName;
            this.lName = lName;

        }

        public string getName()
        {

            return fName + " " + lName;


        }


    }
}
