
using System.Collections.Generic;

namespace PhoenixWebCore.Models
{
    public class Component
    {
        public string Name;
        public string Description;
        public string Brand;
        public double Price;
        public string Type;
        
        public Component()
        {

        }

    }
    public class ProductModel
    {

        
        public List<Component> laptop = new List<Component>();
        public List<Component> tablet = new List<Component>();
        public List<Component> accessory = new List<Component>();

        public ProductModel()
        {

        }
    }

}
