using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductPlatformAnalyzer
{
    public class VariantGroup
    {
        public string Names { get; set; }
        public string GCardinality { get; set; }
        public List<Variant> Variants { get; set; }

        public VariantGroup()
        {

        }

        public VariantGroup(string pNames, string pGCardinality, List<Variant> pVariants)
        {
            Names = pNames;
            GCardinality = pGCardinality;
            Variants = pVariants;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            VariantGroup lVariantGroup = (VariantGroup)obj;
            return (this.Names == lVariantGroup.Names && this.Variants == lVariantGroup.Variants);
        }

        public override int GetHashCode()
        {
            return (this.Names.GetHashCode() * 2 + this.Variants.GetHashCode());
        }
    }
}
