using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductPlatformAnalyzer
{
    public class Action
    {
        public string Name { get; set; }
        public List<string> Precondition { get; set; }
        public string Effect { get; set; }

        public Action(string pName, List<string> pPrecondition, string pEffect)
        {
            Name = pName;
            Precondition = pPrecondition;
            Effect = pEffect;
        }

        public void AddPrecondition(string pPrecondition)
        {
            if (pPrecondition != null)
                Precondition.Add(pPrecondition);
        }


    }
}
