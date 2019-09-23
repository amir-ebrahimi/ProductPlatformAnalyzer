using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductPlatformAnalyzer
{
    public class Variant
    {
        public string Names { get; set; }

        public VariantGroup MyVariantGroup { get; set; }

        public Variant()
        {

        }

        public Variant(string pNames)
        {
            Names = pNames;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Variant lVariant = (Variant)obj;
            return (this.Names == lVariant.Names);
        }

        public override int GetHashCode()
        {
            return this.Names.GetHashCode() * 3;
        }
    }
}
