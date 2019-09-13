using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductPlatformAnalyzer
{
    public class Trait
    {
        public string Names { get; set; }
        public HashSet<Tuple<string,string>> Attributes { get; set; }
        public HashSet<Trait> Inherit { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Trait lTrait = (Trait)obj;
            return (this.Names == lTrait.Names);
        }

        public override int GetHashCode()
        {
            return (this.Names.GetHashCode() * 7);
        }

    }
}
