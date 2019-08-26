using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductPlatformAnalyzer
{
    public class Resource
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        //Hash function then remove dictionary from framework also overload the equals for this object
        public override bool Equals(object pObj)
        {
            Resource lResource = (pObj as Resource);
            if (lResource == null)
                return false;
            else
                return (Name == lResource.Name);
        }
    }
}
