using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductPlatformAnalyzer
{
    public class Part
    {
        public string Names { get; set; }

        public Part()
        {

        }

        public Part(string pNames)
        {
            Names = pNames;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Part lPart = (Part)obj;
            return this.Names == lPart.Names;
        }

        public override int GetHashCode()
        {
            return this.Names.GetHashCode() * 5;
        }
    }
}
