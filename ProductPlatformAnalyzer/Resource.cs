using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductPlatformAnalyzer
{
    public class Resource
    {
        public string Name { get; set; }

        public Resource()
        {

        }

        public Resource(string pName)
        {
            Name = pName;
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
