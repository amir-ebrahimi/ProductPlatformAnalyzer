using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductPlatformAnalyzer
{
    public class PartUsageRule
    {
        public string VariantExpression { get; set; }
        public Part Part { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            PartUsageRule lPartUsageRule = (PartUsageRule)obj;
            return (this.VariantExpression == lPartUsageRule.VariantExpression && this.Part == lPartUsageRule.Part);
        }

        public override int GetHashCode()
        {
            return (this.VariantExpression.GetHashCode() * 8 + this.Part.GetHashCode());
        }

    }
}
